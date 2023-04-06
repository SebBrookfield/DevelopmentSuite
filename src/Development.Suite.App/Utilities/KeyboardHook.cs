using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Development.Suite.App.Utilities;

public sealed class KeyboardHook : IDisposable
{
    private readonly int _id;
    private readonly nint _handle;
    private readonly Dispatcher _dispatcher;
    private bool _registered;
    private Action? _keyPressed;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(nint hWnd, int id, ModifierKeys fsModifiers, int vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(nint hWnd, int id);

    public KeyboardHook(Window window)
    {
        _id = GetHashCode();
        _handle = new WindowInteropHelper(window).EnsureHandle();
        _dispatcher = window.Dispatcher;
    }

    public void RegisterHotKey(ModifierKeys modifierKeys, Key key, Action keyPressed)
    {
        if (_registered)
            throw new ApplicationException("Hot key is already registered.");

        _keyPressed = keyPressed;

        var interopKey = KeyInterop.VirtualKeyFromKey(key);
        _registered = RegisterHotKey(_handle, _id, modifierKeys, interopKey);

        if (!_registered)
            throw new ApplicationException("Failed to register hot key.");

        ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessage;
    }

    public void Dispose()
    {
        try
        {
            ComponentDispatcher.ThreadPreprocessMessage -= ThreadPreprocessMessage;
        }
        catch
        {
            // Ignored
        }
        finally
        {
            UnregisterHotKey(_handle, _id);
        }
    }

    private void ThreadPreprocessMessage(ref MSG msg, ref bool handled)
    {
        const int wmHotKey = 0x0312;
        
        if (handled || msg.message != wmHotKey || (int)(msg.wParam) != _id)
            return;

        _dispatcher.Invoke(() => _keyPressed?.Invoke());

        handled = true;
    }
}