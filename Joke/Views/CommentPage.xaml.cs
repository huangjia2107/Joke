using Joke.Models;
using Joke.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        public CommentPage()
        {
            this.InitializeComponent();
            this.DataContext = CommentVM;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //             if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
            //                 Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            BackBtn_Click(sender, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //             if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
            //                 Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

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
    }
}
