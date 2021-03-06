﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Joke.Models;
using Joke.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Joke.ViewModels
{
    public class JokeViewModel : ViewModelBase
    {
        public JokeViewModel(JokeAPI jokeAPI, User userInfo)
        {
            JokeAPI = jokeAPI;
            UserInfo = userInfo;

            _PageIndex = 1;
            Title = HashMap.JokeTitleMap[jokeAPI];
            LoadJokeInfoCollection(jokeAPI);
        }

        #region Property

        public JokeAPI JokeAPI { get; set; }
        public User UserInfo { get; set; }

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

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; RaisePropertyChanged("Title"); }
        }

        private uint _PageIndex;
        public uint PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; RaisePropertyChanged("PageIndex"); }
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

        private void LoadJokeInfoCollection(JokeAPI jokeAPI)
        {
            JokeInfoCollection = new IncrementalLoadingCollection<JokeInfo>(
               async (pageIndex, requestCount) =>
               {
                   IsBusy = !IsDisConnected;
                   PageIndex = pageIndex;
                   JokeResponse<JokeInfo> tempJokeResponse = await JokeAPIUtils.GetJokeInfoList<JokeInfo>(new RequestParam
                   {
                       jokeAPI = jokeAPI,
                       page = pageIndex,
                       count = requestCount,
                       token = JokeAPIUtils.UserLoginInfo.token,
                       args = UserInfo == null ? null : new string[] { UserInfo.id }
                   });
                   IsBusy = false;

                   return tempJokeResponse;
               },
               (jokeInfo) =>
               {
                   return JokeInfoCollection.FirstOrDefault(joke => joke.id == jokeInfo.id) != null;
               });

            _JokeInfoCollection.OnLoadStatusChanged += _JokeInfoCollection_OnLoadStatusChanged;
            _JokeInfoCollection.OnNetworkStatusChanged += _JokeInfoCollection_OnNetworkStatusChanged;
        }

        private void _JokeInfoCollection_OnLoadStatusChanged(LoadStatusArgs args)
        {
            switch (args.Status)
            {
                case LoadStatus.Empty:

                    Messenger.Default.Send(
                        new PopupToastArgs { IsCancel = false, Msg = "未能获取到任何内容！" },
                        MessageHelper.PopupMainToastToken);

                    break;
                case LoadStatus.Finish:

                    Messenger.Default.Send(
                        new PopupToastArgs { IsCancel = false, Msg = "全部内容已获取完毕！" },
                        MessageHelper.PopupMainToastToken);

                    break;
            }
        }

        private void _JokeInfoCollection_OnNetworkStatusChanged(bool IsDisconnected)
        {
            this.IsDisConnected = IsDisconnected;

            Messenger.Default.Send(
                new PopupToastArgs { IsCancel = !IsDisConnected, Msg = "当前网络异常！" },
                MessageHelper.PopupMainToastToken);
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
                        LoadJokeInfoCollection(JokeAPI);
                    });

                return refreshCommand;
            }
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
                    });

                return goTopCommand;
            }
        }   

        #endregion    
    }
}
