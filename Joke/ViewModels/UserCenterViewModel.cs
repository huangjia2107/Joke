using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Joke.Models;
using Joke.Utils;
using Joke.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

                JokeAPIUtils.Usertoken = value.token;

                if (value.err == 0)
                {
                    LoginGridVisibility = Visibility.Collapsed;
                    InfoGridVisibility = Visibility.Visible;

                    FileHelper.SaveCredential(value.userid, value.userpassword);
                }
                else
                {
                    InfoGridVisibility = Visibility.Collapsed;
                    LoginGridVisibility = Visibility.Visible;

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

        #endregion

        #region Command

        RelayCommand loginBtnCommand { get; set; }
        public ICommand LoginBtnCommand
        {
            get
            {
                if (loginBtnCommand == null)
                    loginBtnCommand = new RelayCommand(async () =>
                    {
                        if (string.IsNullOrEmpty(_UserID) || string.IsNullOrEmpty(_UserPassword))
                            return;

                        LoginInfo resultInfo = await JokeAPIUtils.GetLoginInfo(_UserID, _UserPassword);
                        if (resultInfo != null)
                        {
                            if (resultInfo.err == 0)
                            {
                                resultInfo.userid = _UserID;
                                resultInfo.userpassword = _UserPassword;

                                UserLoginInfo = resultInfo;
                            }
                            else
                            {
                                //Some Error...
                            }
                        }

                    }, CanLoginExecute);

                return loginBtnCommand;
            }
        }

        bool CanLoginExecute()
        {
            return true;
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
                        GoToPage(typeof(UserDetailPage), new UserDetailParam
                        {
                            jokeAPI = JokeAPI.UserDetail,
                            user = UserLoginInfo.user,
                            token = UserLoginInfo.token
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
                        GoToPage(typeof(UserJokePage), new UserCenterParam
                        {
                            jokeAPI = JokeAPI.Publish,
                            loginInfo = UserLoginInfo
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
                        GoToPage(typeof(UserJokePage), new UserCenterParam
                        {
                            jokeAPI = JokeAPI.Participate,
                            loginInfo = UserLoginInfo
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
                        GoToPage(typeof(UserJokePage), new UserCenterParam
                        {
                            jokeAPI = JokeAPI.Collection,
                            loginInfo = UserLoginInfo
                        });
                    });

                return myCollectionCommand;
            }
        }

        #endregion

        #region Func

        private void GoToPage(Type pageType, object parameter = null)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                rootFrame.Navigate(pageType, parameter);
            }
        }

        #endregion
    }
}
