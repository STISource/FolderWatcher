using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FolderWatcher.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is bool))
            {
                return Visibility.Collapsed;
            }

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is Visibility))
            {
                return false;
            }

            return (Visibility)value == Visibility.Visible ? true : false;
        }
    }
}
