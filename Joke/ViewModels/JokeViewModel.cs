using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        public JokeViewModel(JokeAPI jokeAPI)
        {
            JokeAPI = jokeAPI;

            Title = HashMap.JokeTitleMap[jokeAPI];
            LoadJokeInfoCollection(jokeAPI);
        }

        #region Property

        public JokeAPI JokeAPI{ get; set; }

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
                        LoadJokeInfoCollection(JokeAPI);
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
