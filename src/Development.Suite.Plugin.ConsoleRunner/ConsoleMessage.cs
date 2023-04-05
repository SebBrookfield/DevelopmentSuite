namespace Development.Suite.Plugin.ConsoleRunner;

[Serializable]
public class ConsoleMessage : IpcModel
{
    public string? Command { get; set; }
    public string? Reply { get; set; }

    public ConsoleMessage()
    {
    }

    public ConsoleMessage(ConsoleMessage model) : base(model)
    {
        Command = model.Command;
    }
}