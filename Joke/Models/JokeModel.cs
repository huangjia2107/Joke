using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Joke.Utils;
using Joke.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Joke.Models
{
    public class Votes
    {
        public int up { get; set; }
        public int down { get; set; }
    }

    public class ImageSize
    {
        public int[] s { get; set; }
        public int[] m { get; set; }
    }

    public class JokeInfo : ViewModelBase
    {
        //public  
        public string id { get; set; }
        public long created_at { get; set; }
        public long published_at { get; set; }
        public Votes votes { get; set; }
        public string content { get; set; }
        public string tag { get; set; }
        public int comments_count { get; set; }
        public bool is_mine { get; set; }
        public User user { get; set; }
        public string image { get; set; }
        public ImageSize image_size { get; set; }

        //video 
        public string high_url { get; set; }
        public string low_url { get; set; }
        public string pic_url { get; set; }
        public int[] pic_size { get; set; }
        public long loop { get; set; }

        public bool is_anonymous
        {
            get { return user == null ? true : false; }
        }

        public string created_time
        {
            get { return Algorithm.GetRealTime(created_at).ToString("yyyy-MM-dd HH:mm"); }
        }

        public string published_time
        {
            get { return Algorithm.GetRealTime(published_at).ToString("yyyy-MM-dd HH:mm"); }
        }

        public bool IsExistText
        {
            get { return !string.IsNullOrEmpty(content); }
        }

        public bool IsExistImage
        {
            get { return !string.IsNullOrEmpty(image); }
        }

        public bool IsExistVideo
        {
            get { return !(string.IsNullOrEmpty(high_url) && string.IsNullOrEmpty(low_url)); }
        }

        //Image
        public string small_image
        {
            get
            {
                if (string.IsNullOrEmpty(image))
                    return @"ms-Appx:///Assets/Images/default.png";
                else
                    return string.Format("http://img.qiushibaike.com/system/pictures/{0}/{1}/small/{2}", id.Substring(0, 5), id, image);
            }
        }
        public string medium_image
        {
            get
            {
                if (string.IsNullOrEmpty(image))
                    return @"ms-Appx:///Assets/Images/default.png";
                else
                    return string.Format("http://img.qiushibaike.com/system/pictures/{0}/{1}/medium/{2}", id.Substring(0, 5), id, image);
            }
        }

        //Video   
        public string picurl
        {
            get
            {
                if (string.IsNullOrEmpty(pic_url))
                    return @"ms-Appx:///Assets/Images/default.png";
                else
                    return pic_url;
            }
        }

        public string highurl
        {
            get
            {
                if (string.IsNullOrEmpty(high_url))
                    return @"ms-Appx:///Assets/Images/default.png";
                else
                    return high_url;
            }
        }

        public int picwidth
        {
            get
            {
                if (pic_size == null)
                    return 480;
                else
                    return pic_size[0];
            }
        }

        public int picheight
        {
            get
            {
                if (pic_size == null)
                    return 480;
                else
                    return pic_size[1];
            }
        }

        #region No Joke Property

        public bool OpenCommentEnabled { get; set; } = true;

        private bool? _IsPlaying;
        public bool? IsPlaying
        {
            get { return _IsPlaying; }
            set { _IsPlaying = value; RaisePropertyChanged("IsPlaying"); }
        }

        #endregion

        #region Command

        RelayCommand commentCommand { get; set; }
        public ICommand CommentCommand
        {
            get
            {
                if (commentCommand == null)
                    commentCommand = new RelayCommand(() =>
                    {
                        Frame rootFrame = Window.Current.Content as Frame;
                        if (rootFrame != null && OpenCommentEnabled)
                        {
                            rootFrame.Navigate(typeof(CommentPage), this);
                        }
                    });

                return commentCommand;
            }
        }

        RelayCommand upCommand { get; set; }
        public ICommand UpCommand
        {
            get
            {
                if (upCommand == null)
                    upCommand = new RelayCommand(() =>
                    {
                        System.Diagnostics.Debug.WriteLine("Up the joke...");
                    });

                return upCommand;
            }
        }

        RelayCommand downCommand { get; set; }
        public ICommand DownCommand
        {
            get
            {
                if (downCommand == null)
                    downCommand = new RelayCommand(() =>
                    {
                        System.Diagnostics.Debug.WriteLine("Down the joke...");
                    });

                return downCommand;
            }
        }

        RelayCommand<MediaElement> playCommand { get; set; }
        public ICommand PlayCommand
        {
            get
            {
                if (playCommand == null)
                    playCommand = new RelayCommand<MediaElement>((m) =>
                    {
                        if (m.CurrentState == MediaElementState.Playing)
                        {
                            if (m.CanPause)
                            {
                                m.Pause();
                                IsPlaying = false;
                            }
                            else
                                IsPlaying = true;
                        }
                        else
                        {
                            m.Play();
                            IsPlaying = true;
                        }


                        System.Diagnostics.Debug.WriteLine("Play the joke...");
                    });

                return playCommand;
            }
        }

        #endregion 
    }
}
