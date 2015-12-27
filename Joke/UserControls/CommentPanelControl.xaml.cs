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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Joke.UserControls
{
    public sealed partial class CommentPanelControl : UserControl
    {
        CommentViewModel CommentVM { get; set; } = new CommentViewModel();

        public CommentPanelControl()
        {
            this.InitializeComponent();
            this.DataContext = CommentVM;
        }

        public static readonly DependencyProperty CurrentJokeInfoProperty =
            DependencyProperty.Register("CurrentJokeInfo", typeof(JokeInfo), typeof(CommentPanelControl), new PropertyMetadata(null, JokeIDPropertyChangedCallback));
        public JokeInfo CurrentJokeInfo
        {
            get { return (JokeInfo)GetValue(CurrentJokeInfoProperty); }
            set { SetValue(CurrentJokeInfoProperty, value); }
        }
        static void JokeIDPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CommentPanelControl obj = sender as CommentPanelControl;
            if (obj.CurrentJokeInfo != null)
                obj.CommentVM.CurrentJokeInfo = obj.CurrentJokeInfo;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
