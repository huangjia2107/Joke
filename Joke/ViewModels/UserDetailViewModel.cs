using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Joke.Models;
using Joke.Utils;
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
        private UserDetailParam userDetailParam = null;

        public UserDetailViewModel()
        {
            _userDetail = new UserData();
        }

        private UserData _userDetail;
        public UserData userDetail
        {
            get { return _userDetail; }
            set { _userDetail = value; }
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
            IAsyncAction asyncAction = ThreadPool.RunAsync(async (workItem) =>
            {
                if (workItem.Status == AsyncStatus.Canceled)
                    return;

                userDetailParam = param;
                UserResponse userResponse = await JokeAPIUtils.GetObjResult<UserResponse>(new RequestParam
                {
                    jokeAPI = param.jokeAPI,
                    token = param.token,
                    args = new string[] { param.user.id }
                });

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                            new DispatchedHandler(() =>
                            {
                                if (userResponse != null)
                                {
                                    if (userResponse.err == 0)
                                        UpdateUserDetail(userResponse.userdata);
                                }

                                IsBusy = false;
                            }));
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
