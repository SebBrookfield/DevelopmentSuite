using Development.Suite.Logging;

namespace Development.Suite.Plugin.ConsoleRunner;

public class ConsoleMessageHandler : IMessageHandler<ConsoleMessage>
{
    private IDevelopmentSuiteLogger _logger;

    public ConsoleMessageHandler(IDevelopmentSuiteLogger<ConsoleMessageHandler> logger)
    {
        _logger = logger;
    }

    public void HandleMessage(ConsoleMessage message)
    {
        _logger.LogDebug("Messaageeeee", message);
    }
}