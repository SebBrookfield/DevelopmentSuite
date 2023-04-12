using Development.Suite.Logging;

namespace Development.Suite.App.Plugin.ExampleCommand
{
    public class ExampleCommand : IPluginCommand
    {
        public string Name => "Example";
        public string Description => "Shows an example of a command.";
        
        private readonly IDevelopmentSuiteLogger<ExampleCommand> _logger;

        public ExampleCommand(IDevelopmentSuiteLogger<ExampleCommand> logger)
        {
            _logger = logger;
        }

        public Task Execute()
        {
            _logger.LogDebug($"{nameof(Execute)}");
            new ExampleWindow().Show();
            return Task.CompletedTask;
        }
    }
}