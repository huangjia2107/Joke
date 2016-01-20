using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Joke.Utils
{
    public class Algorithm
    {
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        public static DateTime GetRealTime(long timeStamp)
        {
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static async void HideStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
        }

        public static VisualStateGroup FindVisualState(FrameworkElement element, string groupName)
        {
            if (element == null)
                return null;

            List<VisualStateGroup> groups = VisualStateManager.GetVisualStateGroups(element).ToList();
            foreach (VisualStateGroup group in groups)
            {
                if (group.Name == groupName)
                    return group;
            }

            return null;
        }

        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            T returnElement = null;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == typeof(T))
                    {
                        return (T)element;
                    }
                    else
                    {
                        returnElement = FindChild<T>(VisualTreeHelper.GetChild(parent, i));
                    }
                }
            }
            return returnElement;
        }

        public static string GetLoopStr(long loop)
        {
            if (loop > 100000000)
                return string.Format("{0:#.##}亿 播放", loop / 100000000);
            else if (loop > 10000)
                return string.Format("{0:#.#}万 播放", loop / 10000);
            else
                return loop + " 播放";
        }

        public static void SetTheme(FrameworkElement element)
        {
            if (element == null)
                return;

            if (element.RequestedTheme == ElementTheme.Light)
            {
                element.RequestedTheme = ElementTheme.Dark;
                //AppSetting.Instance.IsDarkTheme = true;
            }
            else
            {
                element.RequestedTheme = ElementTheme.Light;
                //AppSetting.Instance.IsDarkTheme = false;
            }

        }

        public static bool IsNetworkValid()
        {
            ConnectionProfile CP = NetworkInformation.GetInternetConnectionProfile();

            if (CP == null)
                return false;

            return CP.IsWlanConnectionProfile || CP.IsWwanConnectionProfile;
        }

        public static void GoToPage(Type pageType, object parameter = null)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                rootFrame.Navigate(pageType, parameter);
            }
        }
    }
}
