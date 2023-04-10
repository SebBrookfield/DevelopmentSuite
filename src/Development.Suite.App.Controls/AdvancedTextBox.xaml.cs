using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Development.Suite.App.Controls
{
    public class AdvancedTextBox : UserControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(AdvancedTextBox), new PropertyMetadata(default(CornerRadius)));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof(Watermark), typeof(string), typeof(AdvancedTextBox), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(AdvancedTextBox), new PropertyMetadata(default(string)));
        
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public new void Focus()
        {
            var searchTermTextBox = FindVisualChild<TextBox>(this, "SearchTermTextBox");
            searchTermTextBox?.Focus();
        }

        private static T? FindVisualChild<T>(DependencyObject parent, string? name = null) where T : FrameworkElement
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);

            if (childCount == 0)
                return null;

            for (var i = 0; i < childCount; i++)
            {
                var childObject = VisualTreeHelper.GetChild(parent, i);

                if (childObject is T child && (name == null || child.Name == name))
                    return child;

                var result = FindVisualChild<T>(childObject, name);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}