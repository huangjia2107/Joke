using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Joke.Models;
using Joke.Utils;
using Joke.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Joke.ViewModels
{
    public class UserCenterViewModel : ViewModelBase
    {
        #region Ctor

        public UserCenterViewModel()
        {
            UserLoginInfo = new LoginInfo();
            _LoginGridVisibility = Visibility.Visible;
            _InfoGridVisibility = Visibility.Collapsed;

            _LoginBtnText = "登录";
        }

        #endregion

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

                if (value.err == 0)
                {
                    LoginGridVisibility = Visibility.Collapsed;
                    InfoGridVisibility = Visibility.Visible;

                    UserID = value.userid;
                    UserPassword = value.userpassword;

                    FileHelper.SaveCredential(value.userid, value.userpassword);
                }
                else
                {
                    InfoGridVisibility = Visibility.Collapsed;
                    LoginGridVisibility = Visibility.Visible;

                    UserID = value.userid = string.Empty;
                    UserPassword = value.userpassword = string.Empty;

                    FileHelper.RemoveCredential();
                }

                Messenger.Default.Send(
                       new UserStatusArgs { UserLoginInfo = value },
                       MessageHelper.UserStatusToken);
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

        private Visibility _LoginGridVisibility;
        public Visibility LoginGridVisibility
        {
            get { return _LoginGridVisibility; }
            set { _LoginGridVisibility = value; RaisePropertyChanged("LoginGridVisibility"); }
        }

        private Visibility _InfoGridVisibility;
        public Visibility InfoGridVisibility
        {
            get { return _InfoGridVisibility; }
            set { _InfoGridVisibility = value; RaisePropertyChanged("InfoGridVisibility"); }
        }

        private string _UserID;
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; RaisePropertyChanged("UserID"); }
        }

        private string _UserPassword;
        public string UserPassword
        {
            get { return _UserPassword; }
            set { _UserPassword = value; RaisePropertyChanged("UserPassword"); }
        }

        private string _LoginBtnText;
        public string LoginBtnText
        {
            get { return _LoginBtnText; }
            set { _LoginBtnText = value; RaisePropertyChanged("LoginBtnText"); }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        #endregion

        #region Command

        RelayCommand loginBtnCommand { get; set; }
        public ICommand LoginBtnCommand
        {
            get
            {
                if (loginBtnCommand == null)
                    loginBtnCommand = new RelayCommand(() =>
                    {
                        if (string.IsNullOrEmpty(_UserID) || string.IsNullOrEmpty(_UserPassword))
                            return;

                        DoLogin(_UserID, _UserPassword);
                    });

                return loginBtnCommand;
            }
        }

        RelayCommand quitBtnCommand { get; set; }
        public ICommand QuitBtnCommand
        {
            get
            {
                if (quitBtnCommand == null)
                    quitBtnCommand = new RelayCommand(() =>
                    {
                        if (UserLoginInfo != null)
                            UserLoginInfo = new LoginInfo();
                    });

                return quitBtnCommand;
            }
        }

        RelayCommand editCommand { get; set; }
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                    editCommand = new RelayCommand(() =>
                    {
                        Algorithm.GoToPage(typeof(UserDetailPage), new UserDetailParam
                        {
                            jokeAPI = JokeAPI.UserDetail,
                            user = UserLoginInfo.user,
                        });
                    });

                return editCommand;
            }
        }

        RelayCommand myPublishCommand { get; set; }
        public ICommand MyPublishCommand
        {
            get
            {
                if (myPublishCommand == null)
                    myPublishCommand = new RelayCommand(() =>
                    {
                        Algorithm.GoToPage(typeof(UserJokePage), new UserJokeParam
                        {
                            jokeAPI = JokeAPI.MyPublish,
                        });
                    });

                return myPublishCommand;
            }
        }

        RelayCommand myParticipateCommand { get; set; }
        public ICommand MyParticipateCommand
        {
            get
            {
                if (myParticipateCommand == null)
                    myParticipateCommand = new RelayCommand(() =>
                    {
                        Algorithm.GoToPage(typeof(UserJokePage), new UserJokeParam
                        {
                            jokeAPI = JokeAPI.MyParticipate,
                        });
                    });

                return myParticipateCommand;
            }
        }

        RelayCommand myCollectionCommand { get; set; }
        public ICommand MyCollectionCommand
        {
            get
            {
                if (myCollectionCommand == null)
                    myCollectionCommand = new RelayCommand(() =>
                    {
                        Algorithm.GoToPage(typeof(UserJokePage), new UserJokeParam
                        {
                            jokeAPI = JokeAPI.MyCollection,
                        });
                    });

                return myCollectionCommand;
            }
        }

        #endregion

        #region Func

        private void DoLogin(string username, string password)
        {
            IsBusy = true;
            LoginBtnText = "正在登录...";
            IAsyncAction asyncAction = ThreadPool.RunAsync(async (workItem) =>
            {
                if (!Algorithm.IsNetworkValid())
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            LoginBtnText = "登录";
                            IsBusy = false;
                            Messenger.Default.Send(
                                       new PopupToastArgs { IsCancel = false, Msg = "网络异常,请重试." },
                                       MessageHelper.PopupUserCenterToastToken);
                        });

                    return;
                }

                LoginInfo resultInfo = await JokeAPIUtils.GetLoginInfo(username, password);
                if (resultInfo != null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if (resultInfo.err == 0)
                        {
                            resultInfo.userid = username;
                            resultInfo.userpassword = password;

                            UserLoginInfo = resultInfo;
                        }
                        else
                        {
                            Messenger.Default.Send(
                                new PopupToastArgs { IsCancel = false, Msg = "账号或密码错误." },
                                MessageHelper.PopupUserCenterToastToken);
                        }

                        LoginBtnText = "登录";
                        IsBusy = false;
                    });
                }
                else
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Messenger.Default.Send(
                                   new PopupToastArgs { IsCancel = false, Msg = "登录失败,请重试." },
                                   MessageHelper.PopupUserCenterToastToken);

                        UserName = "登录";
                        IsBusy = false;
                    });
                }
            });
        }

        #endregion
    }
}
