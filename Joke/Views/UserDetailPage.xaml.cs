using GalaSoft.MvvmLight.Messaging;
using Joke.Utils;
using Joke.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Joke.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserDetailPage : Page
    {
        UserDetailViewModel UserDetailVM { get; set; } = new UserDetailViewModel();
        bool StoryBoardIsBusy = false;

        public UserDetailPage()
        {
            this.InitializeComponent();
            this.DataContext = UserDetailVM;
        }

        private void HandlePopupToastMessage(PopupToastArgs args)
        {
            if (StoryBoardIsBusy)
            {
                StoryBoardIsBusy = false;
                MsgVisibleStoryboard.Stop();
            }

            if (!args.IsCancel)
            {
                StoryBoardIsBusy = true;
                tipText.Text = args.Msg;
                MsgVisibleStoryboard.Begin();
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (ApiInformation.IsTypePresent(PlatformAPIHelper.BackPressedEventArgsAPI))
                e.Handled = true;

            BackBtn_Click(sender, null);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Messenger.Default.Unregister<PopupToastArgs>(this, MessageHelper.PopupUserDetailToastToken, HandlePopupToastMessage);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            Messenger.Default.Register<PopupToastArgs>(this, MessageHelper.PopupUserDetailToastToken, HandlePopupToastMessage);

            UserDetailParam param = e.Parameter as UserDetailParam;
            if (param != null && UserDetailVM != null)
                UserDetailVM.LoadUserDetailInfo(param);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                if (UserDetailVM != null)
                    UserDetailVM.CancelInitUserDetailInfo();

                this.Frame.GoBack();
            }
        }

        private void MsgVisibleStoryboard_Completed(object sender, object e)
        {
            StoryBoardIsBusy = false;
        }
    }
}
