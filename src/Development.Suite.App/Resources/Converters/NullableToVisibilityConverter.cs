using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Development.Suite.App.ExtensionMethods;

namespace Development.Suite.App.Resources.Converters;

[ValueConversion(typeof(Nullable<>), typeof(Visibility))]
public class NullableToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null || value is string str && str.IsNullOrWhitespace() ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}