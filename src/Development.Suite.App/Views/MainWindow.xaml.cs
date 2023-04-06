using System.Windows;

namespace Development.Suite.App.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var measure = base.MeasureOverride(availableSize);
        Top = SystemParameters.FullPrimaryScreenHeight / 4;
        Left = (SystemParameters.FullPrimaryScreenWidth - measure.Width) / 2;
        return measure;
    }
}