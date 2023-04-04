using System.Text.Json;

namespace Development.Suite.Ipc;

public class IpcMessage
{
    public string FullName { get; set; }
    public string Message { get; set; }

    public static IpcMessage ToIpcMessage<TMessage>(TMessage message) where TMessage : class
    {
        var type = typeof(TMessage);

        return new IpcMessage
        {
            FullName = type.FullName ?? type.Name,
            Message = JsonSerializer.Serialize(message)
        };
    }
}