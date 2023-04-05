using Development.Suite.Logging;
using Development.Suite.Plugin;
using Development.Suite.Plugin.ConsoleRunner;

namespace Development.Suite.Service.TestClient;

public class ConsoleReplyHandler : IMessageHandler<ConsoleMessage>
{
    private readonly IDevelopmentSuiteLogger<ConsoleReplyHandler> _logger;

    public ConsoleReplyHandler(IDevelopmentSuiteLogger<ConsoleReplyHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleMessage(ConsoleMessage message)
    {
        _logger.LogInformation(message.Reply);
        return Task.CompletedTask;
    }
}