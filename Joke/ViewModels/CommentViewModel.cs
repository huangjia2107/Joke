using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Joke.Models;
using Joke.Utils;
using Joke.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Joke.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        public CommentViewModel()
        {
            _PageIndex = 1;
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

        private JokeInfo _CurrentJokeInfo;
        public JokeInfo CurrentJokeInfo
        {
            get { return _CurrentJokeInfo; }
            set
            {
                _CurrentJokeInfo = value;
                RaisePropertyChanged("CurrentJokeInfo");

                Title = "糗事" + _CurrentJokeInfo.id;
                RefreshJokeInfoCollection(_CurrentJokeInfo.id);
            }
        }

        private int _TotalCount;
        public int TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; RaisePropertyChanged("TotalCount"); }
        }

        private IncrementalLoadingCollection<Comment> _CommentCollection;
        public IncrementalLoadingCollection<Comment> CommentCollection
        {
            get { return _CommentCollection; }
            set { _CommentCollection = value; RaisePropertyChanged("CommentCollection"); }
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
                        if (_CurrentJokeInfo != null)
                            RefreshJokeInfoCollection(_CurrentJokeInfo.id);

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
                        if (listView != null)
                        {
                            if (listView.Items.Count > 0)
                                listView.ScrollIntoView(listView.Items[0]);
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

        #region Func

        private void RefreshJokeInfoCollection(string jokeID)
        {
            LoadCommentsCollection(JokeAPI.Comment, jokeID);
        }

        private void LoadCommentsCollection(JokeAPI jokeAPI, string jokeID)
        {
            CommentCollection = new IncrementalLoadingCollection<Comment>(
               async (pageIndex, requestCount) =>
               {
                   IsBusy = !IsDisConnected;
                   PageIndex = pageIndex;
                   JokeResponse<Comment> tempJokeResponse = await JokeAPIUtils.GetJokeInfoList<Comment>(new RequestParam
                   {
                       jokeAPI = jokeAPI,
                       page = pageIndex,
                       count = requestCount,
                       args = new string[] { jokeID }
                   });
                   IsBusy = false;

                   return tempJokeResponse;
               },
                (comment) =>
                {
                    return CommentCollection.FirstOrDefault(c => c.id == comment.id) != null;
                });

            _CommentCollection.OnLoadStatusChanged += _JokeInfoCollection_OnLoadStatusChanged;
            _CommentCollection.OnNetworkStatusChanged += _JokeInfoCollection_OnNetworkStatusChanged;
        }

        private void _JokeInfoCollection_OnNetworkStatusChanged(bool IsDisconnected)
        {
            this.IsDisConnected = IsDisconnected;

            if (OnPopupToast != null)
                OnPopupToast(!IsDisconnected, "当前网络异常！");
        }

        private void _JokeInfoCollection_OnLoadStatusChanged(LoadStatusArgs args)
        {
            switch (args.Status)
            {
                case LoadStatus.Empty:

                    if (OnPopupToast != null)
                        OnPopupToast(false, "未能获取到任何内容！");

                    break;
                case LoadStatus.Finish:

                    if (OnPopupToast != null)
                        OnPopupToast(false, "全部评论已获取完毕！");

                    break;
            }
        }

        #endregion

        #region Event

        public delegate void PopupToast(bool IsCancel, string Msg);
        public event PopupToast OnPopupToast;

        #endregion
    }
}
