using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Joke.Triggers
{
    public class PlayBtnVisibilityTrigger : StateTriggerBase
    {
        public Visibility ElementVisibility
        {
            get { return (Visibility)GetValue(ElementVisibilityProperty); }
            set { SetValue(ElementVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ElementVisibilityProperty =
            DependencyProperty.Register("ElementVisibility", typeof(Visibility), typeof(PlayBtnVisibilityTrigger), new PropertyMetadata(Visibility.Collapsed, VisibilityPropertyChangedCallback));

        static void VisibilityPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var obj = sender as PlayBtnVisibilityTrigger;
            obj.SetActive(obj.ElementVisibility == obj.Visibility);
        }

        private Visibility _Visibility;
        public Visibility Visibility
        {
            get { return _Visibility; }
            set
            {
                SetActive(ElementVisibility == (_Visibility = value));
            }
        }

    }
}
