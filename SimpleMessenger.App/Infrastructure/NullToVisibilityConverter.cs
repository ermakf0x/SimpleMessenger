using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SimpleMessenger.App.Infrastructure;

class NullToVisibilityConverter : IValueConverter
{
    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is null ? Visibility.Hidden : Visibility.Visible;

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
