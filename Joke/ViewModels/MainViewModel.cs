using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Joke.Models;
using Joke.Utils;
using Joke.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Joke.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            InitMenuItemCollection();
        }

        #region Property     

        public MenuItem _SelectedMenuItem;
        public MenuItem SelectedMenuItem
        {
            get { return _SelectedMenuItem; }
            set
            {
                _SelectedMenuItem = value;
                RaisePropertyChanged("SelectedMenuItem");
            }
        }

        private ObservableCollection<MenuItem> _MenuItemCollection;
        public ObservableCollection<MenuItem> MenuItemCollection
        {
            get { return _MenuItemCollection; }
            set { _MenuItemCollection = value; RaisePropertyChanged("MenuItemCollection"); }
        }             

        #endregion

        #region Func

        private void InitMenuItemCollection()
        {
            _MenuItemCollection = new ObservableCollection<MenuItem>()
            {
//                 new MenuItem
//                 {
//                     Icon="\uE121",
//                     IconFontSize=25,
//                     Title="最新",
//                     JokeAPI=JokeAPI.Latest
//                 },

                new MenuItem
                {
                    Icon="\uE7AC",
                    IconFontSize=24,
                    Title="热门",
                    JokeAPI=JokeAPI.Hot
                },

                new MenuItem
                {
                    Icon="\uE18E",
                    IconFontSize=22,
                    Title="文字",
                    JokeAPI=JokeAPI.Text
                },

                new MenuItem
                {
                    Icon="\uEB9F",
                    IconFontSize=24,
                    Title="图片",
                    JokeAPI=JokeAPI.Img
                },

                new MenuItem
                {
                    Icon="\uE173",
                    IconFontSize=24,
                    Title="视频",
                    JokeAPI=JokeAPI.Video
                },
            };

            SelectedMenuItem = _MenuItemCollection.FirstOrDefault();
        }     

        #endregion

        #region Command

        RelayCommand<SplitView> menuBtnCommand { get; set; }
        public ICommand MenuBtnCommand
        {
            get
            {
                if (menuBtnCommand == null)
                    menuBtnCommand = new RelayCommand<SplitView>((splitView) =>
                    {
                        if (splitView != null)
                            splitView.IsPaneOpen = !splitView.IsPaneOpen;

                    }, CanGoTopExecute);

                return menuBtnCommand;
            }
        }

        bool CanGoTopExecute(SplitView splitView)
        {
            return true;
        }

        RelayCommand openUserCommand { get; set; }
        public ICommand OpenUserCommand
        {
            get
            {
                if (openUserCommand == null)
                    openUserCommand = new RelayCommand(() =>
                    {
                        Frame rootFrame = Window.Current.Content as Frame;
                        if (rootFrame != null)
                        {
                            rootFrame.Navigate(typeof(UserPage), this);
                        }
                    }, CanOpenUserExecute);

                return openUserCommand;
            }
        }

        bool CanOpenUserExecute()
        {
            return true;
        }

        #endregion
    }
}
