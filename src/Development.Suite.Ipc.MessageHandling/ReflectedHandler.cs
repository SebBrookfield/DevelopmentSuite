using System.Reflection;
using Development.Suite.Ipc.Common;

namespace Development.Suite.Ipc.MessageHandling;

public class ReflectedHandler : IServiceMessageHandler<IpcModel>
{
    public string Name { get; }

    private readonly MethodInfo? _method;
    private readonly object _handler;

    public ReflectedHandler(object handler)
    {
        var handlerType = handler.GetType();
        _handler = handler;
        _method = handlerType.GetMethod(nameof(HandleMessage));
        
        Name = handlerType.FullName ?? handlerType.Name;
    }

    public async Task HandleMessage(IpcModel message)
    {
        if (_method == null)
            return;

        var task = _method.Invoke(_handler, new object?[] { message }) as Task;

        if (task == null) 
            return;

        await task;
    }
}