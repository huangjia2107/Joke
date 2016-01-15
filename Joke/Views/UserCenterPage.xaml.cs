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
    public sealed partial class UserCenterPage : Page
    {
        UserCenterViewModel UserCenterVM { get; set; } = new UserCenterViewModel();
        public UserCenterPage()
        {
            this.InitializeComponent();
            this.DataContext = UserCenterVM;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoginInfo loginInfo = e.Parameter as LoginInfo;
            if (loginInfo != null)
                UserCenterVM.UserLoginInfo = loginInfo;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }
}
