using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Joke.Data;
using Joke.Models;
using Joke.Utils;
using Joke.ViewModels;
using Joke.Views;
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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Joke
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainViewModel MainVM { get; set; }
        bool StoryBoardIsBusy = false;

        public MainPage()
        {
            this.InitializeComponent();
            DispatcherHelper.Initialize();

            Messenger.Default.Register<PopupToastArgs>(this, MessageHelper.PopupMainToastToken, HandlePopupToastMessage);
            Messenger.Default.Register<UserStatusArgs>(this, MessageHelper.UserStatusToken, HandleUserStatusMessage);

            MainVM = new MainViewModel();
            rootGrid.DataContext = MainVM;
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

        private void HandleUserStatusMessage(UserStatusArgs args)
        {
            if (args == null)
                return;

            if (args.UserLoginInfo != null)
                MainVM.UserLoginInfo = args.UserLoginInfo;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (rootGridVSG.CurrentState != null)
            {
                switch (rootGridVSG.CurrentState.Name)
                {
                    case "WideState":
                        break;
                    case "MiddleState":
                    case "NarrowState":
                        MainSplitView.IsPaneOpen = false;
                        break;
                }
            }
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rootGridVSG.CurrentState != null)
            {
                switch (rootGridVSG.CurrentState.Name)
                {
                    case "WideState":
                        break;
                    case "MiddleState":
                    case "NarrowState":
                        MainSplitView.IsPaneOpen = MainSplitView.IsPaneOpen ? false : MainSplitView.IsPaneOpen;
                        break;
                }
            }

            MenuItem menuItem = MenuListView.SelectedItem as MenuItem;
            if (menuItem != null)
            {
                switch (menuItem.JokeAPI)
                {
                    case JokeAPI.Suggest:
                        ContentFrame.Navigate(typeof(JokeSuggestPage));
                        break;
                    case JokeAPI.Hot:
                        ContentFrame.Navigate(typeof(JokeHotPage));
                        break;
                    case JokeAPI.Text:
                        ContentFrame.Navigate(typeof(JokeTextPage));
                        break;
                    case JokeAPI.Img:
                        ContentFrame.Navigate(typeof(JokeImagePage));
                        break;
                    case JokeAPI.Video:
                        ContentFrame.Navigate(typeof(JokeVideoPage));
                        break;
                }
            }
        }

        private void ThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.Resources["appSettings"] as AppSetting).IsDarkTheme = !(Application.Current.Resources["appSettings"] as AppSetting).IsDarkTheme;
        }

        private void MsgVisibleStoryboard_Completed(object sender, object e)
        {
            StoryBoardIsBusy = false;
        }
    }
}
