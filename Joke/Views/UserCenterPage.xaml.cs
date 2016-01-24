using GalaSoft.MvvmLight.Messaging;
using Joke.Models;
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
    public sealed partial class UserCenterPage : Page
    {
        UserCenterViewModel UserCenterVM { get; set; } = new UserCenterViewModel();
        bool StoryBoardIsBusy = false;

        public UserCenterPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<PopupToastArgs>(this, MessageHelper.PopupUserCenterToastToken, HandlePopupToastMessage);

            this.DataContext = UserCenterVM;
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            LoginInfo loginInfo = e.Parameter as LoginInfo;
            if (loginInfo != null)
            {
                if (loginInfo.err == -1)
                    return;

                UserCenterVM.UserLoginInfo = loginInfo;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }

        private void MsgVisibleStoryboard_Completed(object sender, object e)
        {
            StoryBoardIsBusy = false;
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && UserCenterVM != null)
            {
                if (UserCenterVM.LoginBtnCommand.CanExecute(null))
                    UserCenterVM.LoginBtnCommand.Execute(null);
            }
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && UserCenterVM != null)
            {
                if (UserCenterVM.LoginBtnCommand.CanExecute(null))
                    UserCenterVM.LoginBtnCommand.Execute(null);
            }
        }
    }
}
