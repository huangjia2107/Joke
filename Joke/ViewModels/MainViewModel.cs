using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Joke.Models;
using Joke.Utils;
using Joke.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Joke.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IAsyncAction m_workItem;
        public MainViewModel()
        {
            UserLoginInfo = new LoginInfo();
            InitMenuItemCollection();
            Init();
        }

        private void Init()
        {
            IsBusy = true;
            UserName = "正在登录...";
            IAsyncAction asyncAction = ThreadPool.RunAsync(async (workItem) =>
            {

                //           if (workItem.Status == AsyncStatus.Canceled)
                //               break;
                //                 if (Algorithm.IsNetworkValid() == false)
                //                     return;

                //                 await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                //                          new DispatchedHandler(() =>
                //                          {
                //                              IsBusy = true;
                //                              UserName = "正在登录...";
                //                          }));

                IReadOnlyList<PasswordCredential> pcList = FileHelper.RetrieveAllCredential();
                if (pcList != null)
                {
                    pcList[0].RetrievePassword();
                    LoginInfo resultInfo = await JokeAPIUtils.GetLoginInfo(pcList[0].UserName, pcList[0].Password);
                    if (resultInfo != null)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (resultInfo.err == 0)
                            {
                                resultInfo.userid = pcList[0].UserName;
                                resultInfo.userpassword = pcList[0].Password;

                                UserLoginInfo = resultInfo;
                            }
                            else
                                UserName = "登录";

                            IsBusy = false;
                        });
                    }
                    else
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            UserName = "登录";
                            IsBusy = false;
                        });
                    }
                }
                else
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        UserName = "登录";
                        IsBusy = false;
                    });
                }
            });

            m_workItem = asyncAction;
        }

        #region Property     

        private LoginInfo _UserLoginInfo;
        public LoginInfo UserLoginInfo
        {
            get { return _UserLoginInfo; }
            set
            {
                _UserLoginInfo = value;

                UserName = value.user.login;
                UserPic = value.user.real_icon;

                JokeAPIUtils.UserLoginInfo = value;
            }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; RaisePropertyChanged("UserName"); }
        }

        private string _UserPic;
        public string UserPic
        {
            get { return _UserPic; }
            set { _UserPic = value; RaisePropertyChanged("UserPic"); }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        public MenuItem _SelectedMenuItem;
        public MenuItem SelectedMenuItem
        {
            get { return _SelectedMenuItem; }
            set
            {
                _SelectedMenuItem = value;
                RaisePropertyChanged("SelectedMenuItem");
            }
        }

        private ObservableCollection<MenuItem> _MenuItemCollection;
        public ObservableCollection<MenuItem> MenuItemCollection
        {
            get { return _MenuItemCollection; }
            set { _MenuItemCollection = value; RaisePropertyChanged("MenuItemCollection"); }
        }

        #endregion

        #region Func

        private void InitMenuItemCollection()
        {
            _MenuItemCollection = new ObservableCollection<MenuItem>()
            {
                new MenuItem
                {
                    Icon="\uE12A",
                    IconFontSize=24,
                    Title="图文",
                    JokeAPI=JokeAPI.Suggest
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

                new MenuItem
                {
                    Icon="\uE7AC",
                    IconFontSize=24,
                    Title="热门",
                    JokeAPI=JokeAPI.Hot
                },

//                 new MenuItem
//                 {
//                      Icon="\uE121",
//                      IconFontSize=25,
//                      Title="最新",
//                      JokeAPI=JokeAPI.Latest
//                 }
            };

            SelectedMenuItem = _MenuItemCollection.FirstOrDefault();
        }

        #endregion

        #region Command

        RelayCommand<SplitView> menuBtnCommand { get; set; }
        public ICommand MenuBtnCommand
        {
            get
            {
                if (menuBtnCommand == null)
                    menuBtnCommand = new RelayCommand<SplitView>((splitView) =>
                    {
                        if (splitView != null)
                            splitView.IsPaneOpen = !splitView.IsPaneOpen;
                    });

                return menuBtnCommand;
            }
        }

        RelayCommand openUserCommand { get; set; }
        public ICommand OpenUserCommand
        {
            get
            {
                if (openUserCommand == null)
                    openUserCommand = new RelayCommand(() =>
                    {
                        Algorithm.GoToPage(typeof(UserCenterPage), this.UserLoginInfo);
                    });

                return openUserCommand;
            }
        }

        #endregion
    }
}
