namespace Development.Suite.Plugin.ConsoleRunner;

public class ConsoleMessage : IpcModel
{
    public string Command { get; set; }
    public string Reply { get; set; }
}