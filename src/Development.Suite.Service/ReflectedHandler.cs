using Development.Suite.Plugin;

namespace Development.Suite.Service;

public class ReflectedHandler : IMessageHandler<object>
{
    public string Name { get; }

    private readonly Action<object> _handle;

    public ReflectedHandler(object handler)
    {
        var handlerType = handler.GetType();
        var method = handlerType.GetMethod(nameof(HandleMessage));
        _handle = message => method?.Invoke(handler, new[] { message });
        Name = handlerType.FullName ?? handlerType.Name;
    }

    public void HandleMessage(object message)
    {
        _handle.Invoke(message);
    }
}