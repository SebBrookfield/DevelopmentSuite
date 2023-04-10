using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Development.Suite.App.Resources.Converters;

public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            bool boolean => boolean ? Visibility.Collapsed : Visibility.Visible,
            _ => Visibility.Visible
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}