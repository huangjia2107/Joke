using Joke.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Joke.Utils
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            bool isTrue = (bool)(value ?? false);
            Visibility v = (Visibility)(parameter ?? Visibility.Visible);

            if (isTrue)
                return v;
            else
                return v == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    //     public class BoolToVisibilityConverter : IValueConverter
    //     {
    //         public Visibility TrueVisibility { get; set; }
    // 
    //         public BoolToVisibilityConverter()
    //         {
    //             TrueVisibility = Visibility.Visible;
    //         }
    // 
    //         public object Convert(object value, Type targetType, object parameter, string culture)
    //         {
    //             bool isTrue = (bool)(value??false);
    // 
    //             if (isTrue)
    //                 return TrueVisibility;
    //             else
    //                 return TrueVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    //         }
    // 
    //         public object ConvertBack(object value, Type targetType, object parameter, string culture)
    //         {
    //             throw new NotImplementedException();
    //         }
    //     }

    public class BoolToNotboolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            bool isTrue = (bool)value;

            return !isTrue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringFormatConverter : IValueConverter
    {
        public string StringFormat { get; set; }

        public StringFormatConverter()
        {
            StringFormat = "{0}";
        }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            string text = value.ToString();
            return string.Format(StringFormat, text);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BufferingMediaStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            MediaElementState state = (MediaElementState)value;
            if (state == MediaElementState.Buffering)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ClosedMediaStateToVisibilityConverter : IValueConverter
    {
        public Visibility ClosedVisibility { get; set; }
        public ClosedMediaStateToVisibilityConverter()
        {
            ClosedVisibility = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            MediaElementState state = (MediaElementState)value;
            if (state == MediaElementState.Closed)
                return ClosedVisibility;
            else
                return ClosedVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BufferingProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            double progress = (double)value;
            return string.Format("{0:#}%", progress * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            Visibility visibility = (Visibility)value;
            if (visibility == Visibility.Visible)
                return 0;
            else
                return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MenuItemToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            MenuItem item = (MenuItem)value;
            return item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            MenuItem item = (MenuItem)value;
            return item;
        }
    }

    public class BoolToThemeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool IsTrue = (bool)(value ?? false);

            if (IsTrue)
                return ElementTheme.Dark;
            else
                return ElementTheme.Light;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
