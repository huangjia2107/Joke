using Joke.Utils;
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
    public sealed partial class UserDetailPage : Page
    {
        UserDetailViewModel UserDetailVM { get; set; } = new UserDetailViewModel();

        public UserDetailPage()
        {
            this.InitializeComponent();
            this.DataContext = UserDetailVM;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UserDetailParam param = e.Parameter as UserDetailParam;
            if (param != null && UserDetailVM != null)
                UserDetailVM.LoadUserDetailInfo(param);
        }
    }
}
