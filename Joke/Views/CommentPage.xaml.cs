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
    public sealed partial class CommentPage : Page
    {
        CommentViewModel CommentVM { get; set; } = new CommentViewModel();
        bool StoryBoardIsBusy = false;

        public CommentPage()
        {
            this.InitializeComponent();

            CommentVM.OnPopupToast += CommentVM_OnPopupToast;
            this.DataContext = CommentVM;
        }

        private void CommentVM_OnPopupToast(bool IsDisconnected, string Msg)
        {
            if (IsDisconnected)
            {
                if (StoryBoardIsBusy)
                    return;

                StoryBoardIsBusy = true;
                tipText.Text = Msg;
                MsgVisibleStoryboard.Begin();

            }
            else
            {
                if (StoryBoardIsBusy)
                {
                    StoryBoardIsBusy = false;
                    MsgVisibleStoryboard.Stop();
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (ApiInformation.IsTypePresent(PlatformAPIHelper.BackPressedEventArgsAPI))
                e.Handled = true;

            BackBtn_Click(sender, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            if (CommentVM == null)
                return;

            CommentVM.CurrentJokeInfo = e.Parameter as JokeInfo;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(MainPage), CommentVM.CurrentJokeInfo.id);
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }

        private void MsgVisibleStoryboard_Completed(object sender, object e)
        {
            StoryBoardIsBusy = false;
        }
    }
}
