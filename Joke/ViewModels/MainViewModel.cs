using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Joke.Models;
using Joke.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Joke.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            InitMenuItemCollection();
        }

        #region Property  

        private bool _IsDisConnected;
        public bool IsDisConnected
        {
            get { return _IsDisConnected; }
            set { _IsDisConnected = value; RaisePropertyChanged("IsDisConnected"); }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        private ObservableCollection<MenuItem> _MenuItemCollection;
        public ObservableCollection<MenuItem> MenuItemCollection
        {
            get { return _MenuItemCollection; }
            set { _MenuItemCollection = value; RaisePropertyChanged("MenuItemCollection"); }
        }

        public MenuItem _SelectedMenuItem;
        public MenuItem SelectedMenuItem
        {
            get { return _SelectedMenuItem; }
            set
            {
                _SelectedMenuItem = value;

                //RaisePropertyChanged("SelectedMenuItem");
                LoadJokeInfoCollection((value as MenuItem).JokeAPI);

            }
        }

        private int _TotalCount;
        public int TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; RaisePropertyChanged("TotalCount"); }
        }

        private IncrementalLoadingCollection<JokeInfo> _JokeInfoCollection;
        public IncrementalLoadingCollection<JokeInfo> JokeInfoCollection
        {
            get { return _JokeInfoCollection; }
            set { _JokeInfoCollection = value; RaisePropertyChanged("JokeInfoCollection"); }
        }

        private JokeInfo _SelectedJokeInfo;
        public JokeInfo SelectedJokeInfo
        {
            get { return _SelectedJokeInfo; }
            set { _SelectedJokeInfo = value; RaisePropertyChanged("SelectedJokeInfo"); }
        }


        #endregion

        #region Func

        private void InitMenuItemCollection()
        {
            _MenuItemCollection = new ObservableCollection<MenuItem>()
            {
//                 new MenuItem
//                 {
//                     Icon="\uE121",
//                     IconFontSize=25,
//                     Title="最新",
//                     JokeAPI=JokeAPI.Latest
//                 },

                new MenuItem
                {
                    Icon="\uE7AC",
                    IconFontSize=24,
                    Title="热门",
                    JokeAPI=JokeAPI.Hot
                },

                new MenuItem
                {
                    Icon="\uE18E",
                    IconFontSize=22,
                    Title="文字",
                    JokeAPI=JokeAPI.Text
                },

                new MenuItem
                {
                    Icon="\uEB9F",
                    IconFontSize=24,
                    Title="图片",
                    JokeAPI=JokeAPI.Img
                },

                new MenuItem
                {
                    Icon="\uE173",
                    IconFontSize=24,
                    Title="视频",
                    JokeAPI=JokeAPI.Video
                },
            };

            SelectedMenuItem = _MenuItemCollection.FirstOrDefault();
        }

        private void RefreshJokeInfoCollection()
        {
            if (_SelectedMenuItem != null)
            {
                LoadJokeInfoCollection((_SelectedMenuItem as MenuItem).JokeAPI);
            }
        }

        private void LoadJokeInfoCollection(JokeAPI jokeAPI)
        {
            JokeInfoCollection = new IncrementalLoadingCollection<JokeInfo>(
               async (pageIndex, requestCount) =>
               {
                   IsBusy = !IsDisConnected;
                   JokeResponse<JokeInfo> tempJokeResponse = await JokeAPIUtils.GetJokeInfoList<JokeInfo>(jokeAPI, pageIndex, requestCount);
                   IsBusy = false;

                   return tempJokeResponse;

               });

            _JokeInfoCollection.OnLoadFinished += _JokeInfoCollection_OnLoadFinished;
            _JokeInfoCollection.OnNetworkStatusChanged += _JokeInfoCollection_OnNetworkStatusChanged;
        }

        private void _JokeInfoCollection_OnNetworkStatusChanged(bool IsDisconnected)
        {
            this.IsDisConnected = IsDisconnected;

            if (OnPopupToast != null)
                OnPopupToast(IsDisconnected, "当前网络异常！");
        }

        private void _JokeInfoCollection_OnLoadFinished(int responseTotalCount, int realTotalCount)
        {
            //Finish.
        }

        #endregion

        #region Command

        RelayCommand refreshCommand { get; set; }
        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                    refreshCommand = new RelayCommand(() =>
                    {
                        RefreshJokeInfoCollection();
                    }, CanRefreshExecute);

                return refreshCommand;
            }
        }

        bool CanRefreshExecute()
        {
            return true;
        }

        RelayCommand<ListView> goTopCommand { get; set; }
        public ICommand GoTopCommand
        {
            get
            {
                if (goTopCommand == null)
                    goTopCommand = new RelayCommand<ListView>((listView) =>
                    {
                        if (_JokeInfoCollection != null)
                        {
                            if (listView.Items.Count > 0 && _JokeInfoCollection.Count > 0)
                            {
                                JokeInfo joke = _JokeInfoCollection.FirstOrDefault();
                                if (joke != null)
                                    listView.ScrollIntoView(joke);
                            }

                        }

                    }, CanGoTopExecute);

                return goTopCommand;
            }
        }

        bool CanGoTopExecute(ListView listView)
        {
            return true;
        }

        #endregion

        #region Event

        public delegate void PopupToast(bool IsDisconnected, string Msg);
        public event PopupToast OnPopupToast;

        #endregion
    }
}
