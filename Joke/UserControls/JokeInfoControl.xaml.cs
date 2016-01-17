using Joke.Models;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Joke.UserControls
{
    public sealed partial class JokeInfoControl : UserControl
    {
        public JokeInfo JokeInfo { get { return this.DataContext as JokeInfo; } }

        //用于更新进度
        DispatcherTimer timer = null;
        bool IsStoryboaredBusy = false;

        public JokeInfoControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += timer_Tick;
        }    

        private void timer_Tick(object sender, object e)
        {
            if (mediaPlayer.NaturalDuration.TimeSpan.Seconds == 0)
                return;

            PlayProgressBar_Center.Value = mediaPlayer.Position.TotalMilliseconds * 100 / mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            RemainingTimeTextBlock.Text = (mediaPlayer.NaturalDuration - mediaPlayer.Position).TimeSpan.ToString(@"mm\:ss");
        }

        private void Storyboard_Completed(object sender, object e)
        {
            IsStoryboaredBusy = false;
        }

        private void mediaPlayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (mediaPlayer.CurrentState)
            {
                case MediaElementState.Closed:
                case MediaElementState.Paused:
                case MediaElementState.Stopped:
                    if (timer != null)
                        timer.Stop();
                    break;
                case MediaElementState.Opening:
                case MediaElementState.Buffering:
                case MediaElementState.Playing:
                    if (timer != null)
                        timer.Start();
                    break;
            }
        }

        private void mediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            RemainingTimeTextBlock.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
        }

        private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(0);
            RemainingTimeTextBlock.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            PlayProgressBar_Center.Value = 0;
            PlayBtn.IsChecked = false;
        }

        private void MediaPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsStoryboaredBusy == true)
                return;

            IsStoryboaredBusy = true;

            if (PlayProgressBar_Top.Visibility == Visibility.Collapsed)
                PlayControlCollapsedStoryboard.Begin();
            else
                PlayControlVisibleStoryboard.Begin();
        }

        private void PlayBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
