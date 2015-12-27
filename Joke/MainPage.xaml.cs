using Joke.Data;
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

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Joke
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //             if (ApiInformation.IsTypePresent(PlatformAPIHelper.HardwareButtonsAPI))
        //                 Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed; 

        MainViewModel MainVM { get; set; } = new MainViewModel();
        bool StoryBoardIsBusy = false;


        public MainPage()
        {
            this.InitializeComponent();

            MainVM.OnPopupToast += MainVM_OnPopupToast;
            rootGrid.DataContext = MainVM;      
        }

        private void MainVM_OnPopupToast(bool IsDisconnected, string Msg)
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

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
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
        }

        private void ThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            //                          if (this.RequestedTheme == ElementTheme.Light)
            //                              this.RequestedTheme = ElementTheme.Dark;
            //                          else
            //                              this.RequestedTheme = ElementTheme.Light;

            (Application.Current.Resources["appSettings"] as AppSetting).IsDarkTheme = !(Application.Current.Resources["appSettings"] as AppSetting).IsDarkTheme;

        }

        private void MsgVisibleStoryboard_Completed(object sender, object e)
        {
            StoryBoardIsBusy = false;
        }
    }
}
