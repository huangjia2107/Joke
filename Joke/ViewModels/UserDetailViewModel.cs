﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Joke.Models;
using Joke.Utils;
using Joke.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.System.Threading;
using Windows.UI.Core;

namespace Joke.ViewModels
{
    public class UserDetailViewModel : ViewModelBase
    {
        private IAsyncAction m_workItem = null;

        public UserDetailViewModel()
        {
            _userDetail = new UserData();
        }

        private UserDetailParam _userDetailParam;
        public UserDetailParam userDetailParam
        {
            get { return _userDetailParam; }
            set
            {
                _userDetailParam = value;
                if (value != null)
                    IsMine = value.IsMine;
            }
        }

        private UserData _userDetail;
        public UserData userDetail
        {
            get { return _userDetail; }
            set { _userDetail = value; }
        }

        private bool _IsMine;
        public bool IsMine
        {
            get { return _IsMine; }
            set { _IsMine = value; RaisePropertyChanged("IsMine"); }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; RaisePropertyChanged("IsBusy"); }
        }

        #region Command

        RelayCommand refreshCommand { get; set; }
        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                    refreshCommand = new RelayCommand(() =>
                    {
                        if (userDetailParam != null)
                            LoadUserDetailInfo(userDetailParam);
                    });

                return refreshCommand;
            }
        }

        RelayCommand userPublishCommand { get; set; }
        public ICommand UserPublishCommand
        {
            get
            {
                if (userPublishCommand == null)
                    userPublishCommand = new RelayCommand(() =>
                    {
                        if (IsMine)
                            return;

                        if (string.IsNullOrEmpty(JokeAPIUtils.UserLoginInfo.token))
                        {
                            Messenger.Default.Send(
                                       new PopupToastArgs { Msg = "登录后,即可查看." },
                                       MessageHelper.PopupUserDetailToastToken);
                            return;//需要登录
                        }

                        Algorithm.GoToPage(typeof(UserJokePage), new UserJokeParam
                        {
                            jokeAPI = JokeAPI.UserPublish,
                            user = userDetailParam.user,
                        });
                    });

                return userPublishCommand;
            }
        }

        RelayCommand userParticipateCommand { get; set; }
        public ICommand UserParticipateCommand
        {
            get
            {
                if (userParticipateCommand == null)
                    userParticipateCommand = new RelayCommand(() =>
                    {
                        if (IsMine)
                            return;

                        if (string.IsNullOrEmpty(JokeAPIUtils.UserLoginInfo.token))
                        {
                            Messenger.Default.Send(
                                       new PopupToastArgs { Msg = "登录后,即可查看." },
                                       MessageHelper.PopupUserDetailToastToken);
                            return;//需要登录
                        }

                        Algorithm.GoToPage(typeof(UserJokePage), new UserJokeParam
                        {
                            jokeAPI = JokeAPI.UserParticipate,
                            user = userDetailParam.user,
                        });
                    });

                return userParticipateCommand;
            }
        }

        #endregion

        #region Func

        public void CancelInitUserDetailInfo()
        {
            if (m_workItem != null)
            {
                m_workItem.Cancel();
                m_workItem = null;
            }
        }

        public void LoadUserDetailInfo(UserDetailParam param)
        {
            IsBusy = true;
            userDetailParam = param;

            IAsyncAction asyncAction = ThreadPool.RunAsync(async (workItem) =>
            {
                if (workItem.Status == AsyncStatus.Canceled)
                    return;

                if (!Algorithm.IsNetworkValid())
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        IsBusy = false;
                        Messenger.Default.Send(
                                   new PopupToastArgs { Msg = "网络异常,请重试." },
                                   MessageHelper.PopupUserDetailToastToken);
                    });

                    return;
                }

                UserResponse userResponse = await JokeAPIUtils.GetObjResult<UserResponse>(new RequestParam
                {
                    jokeAPI = param.jokeAPI,
                    token = JokeAPIUtils.UserLoginInfo.token,
                    args = new string[] { param.user.id }
                });

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (userResponse != null)
                    {
                        if (userResponse.err == 0)
                        {
                            UpdateUserDetail(userResponse.userdata);
                            IsBusy = false;

                            return;
                        }
                    }

                    IsBusy = false;
                    Messenger.Default.Send(new PopupToastArgs { Msg = "加载用户信息失败,请重试." },
                                      MessageHelper.PopupUserDetailToastToken);
                });
            });

            m_workItem = asyncAction;
        }

        private void UpdateUserDetail(UserData userData)
        {
            if (userDetail == null || userData == null)
                return;

            userDetail.real_icon = userData.real_icon;
            userDetail.login = userData.login;
            userDetail.gender = userData.gender;
            userDetail.age = userData.age;
            userDetail.signature = userData.signature;
            userDetail.hobby = userData.hobby;
            userDetail.job = userData.job;
            userDetail.haunt = userData.haunt;
            userDetail.hometown = userData.hometown;
            userDetail.introduce = userData.introduce;
            userDetail.mobile_brand = userData.mobile_brand;
            userDetail.created_at = userData.created_at;
            userDetail.qs_cnt = userData.qs_cnt;
            userDetail.smile_cnt = userData.smile_cnt;
        }

        #endregion
    }
}
