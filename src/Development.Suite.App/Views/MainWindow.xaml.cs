using System;
using Development.Suite.App.Utilities;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Development.Suite.App.ViewModels;

namespace Development.Suite.App.Views;

public partial class MainWindow
{
    private readonly KeyboardHook _keyboardHook;

    public MainWindow()
    {
        InitializeComponent();
        _keyboardHook = new KeyboardHook(this);
        _keyboardHook.RegisterHotKey(ModifierKeys.Control | ModifierKeys.Alt | ModifierKeys.Shift, Key.M, SetToForeground);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var measure = base.MeasureOverride(availableSize);
        Top = SystemParameters.FullPrimaryScreenHeight / 4;
        Left = (SystemParameters.FullPrimaryScreenWidth - measure.Width) / 2;
        return measure;
    }

    protected override void OnContentRendered(EventArgs e)
    {
        SetToForeground();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _keyboardHook.Dispose();

        if (DataContext is MainViewModel mainViewModel)
            mainViewModel.OnClose();
    }

    public void SetToBackground()
    {
        WindowState = WindowState.Minimized;
        ShowInTaskbar = false;

        if (DataContext is MainViewModel mainViewModel)
            mainViewModel.OnSetToBackground();
    }

    private void SetToForeground()
    {
        WindowState = WindowState.Normal;
        ShowInTaskbar = true;
        Activate();
        SearchBox.Focus();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape) 
            SetToBackground();
    }
}