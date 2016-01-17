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

namespace Joke.Models
{
    public class Comment
    {
        public string content { get; set; }
        public int floor { get; set; }
        public string id { get; set; }
        public User user { get; set; }

        RelayCommand userDetailCommand { get; set; }
        public ICommand UserDetailCommand
        {
            get
            {
                if (userDetailCommand == null)
                    userDetailCommand = new RelayCommand(() =>
                    {
                        Frame rootFrame = Window.Current.Content as Frame;
                        if (rootFrame != null)
                        {
                            rootFrame.Navigate(typeof(UserDetailPage), new UserDetailParam
                            {
                                jokeAPI = JokeAPI.UserDetail,
                                user = user,
                                token = JokeAPIUtils.Usertoken,
                                IsMine = false
                            });
                        }

                    });

                return userDetailCommand;
            }
        }
    }
}
