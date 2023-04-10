using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Development.Suite.App.Resources.Converters;

public class EnumerableToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable enumerable && enumerable.GetEnumerator().MoveNext())
            return Visibility.Visible;

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}