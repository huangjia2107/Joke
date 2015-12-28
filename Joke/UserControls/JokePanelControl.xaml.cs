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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Joke.UserControls
{
    public sealed partial class JokePanelControl : UserControl
    {
        JokeViewModel JokeVM { get; set; }
        bool StoryBoardIsBusy = false;

        public JokePanelControl(JokeAPI jokeAPI)
        {
            this.InitializeComponent();

            JokeVM = new JokeViewModel(jokeAPI);
            JokeVM.OnPopupToast += JokeVM_OnPopupToast;

            this.DataContext = JokeVM;
        }

        private void JokeVM_OnPopupToast(bool IsDisconnected, string Msg)
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

        private void MsgVisibleStoryboard_Completed(object sender, object e)
        {
            StoryBoardIsBusy = false;
        }    
    }
}
