using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Development.Suite.App.Controls;
using Development.Suite.Common.ExtensionMethods;

namespace Development.Suite.App.Resources.Converters;

public class BoldHighlightFromAdvancedTextBoxConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string text)
            return null;

        if (parameter is not AdvancedTextBox textBox || textBox.Text.IsNullOrWhitespace())
            return text;

        var textBlock = new TextBlock();

        while (text.IndexOf(textBox.Text, StringComparison.InvariantCultureIgnoreCase) != -1)
        {
            var index = text.IndexOf(textBox.Text, StringComparison.InvariantCultureIgnoreCase);
            var textTillSearchTerm = text[..index];
            textBlock.Inlines.Add(CreateRun(textTillSearchTerm, FontWeights.Normal));
            var searchTermInText = text.Substring(index, textBox.Text.Length);
            textBlock.Inlines.Add(CreateRun(searchTermInText, FontWeights.Bold));
            text = text[(index + textBox.Text.Length)..];
        }

        textBlock.Inlines.Add(CreateRun(text, FontWeights.Normal));

        return textBlock;
    }

    private static Run CreateRun(string text, FontWeight fontWeight)
    {
        return new Run(text)
        {
            FontWeight = fontWeight
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}