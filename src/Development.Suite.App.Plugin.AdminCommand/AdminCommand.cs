using System.Security.Principal;
using Development.Suite.Logging;
using Development.Suite.Plugin.ConsoleRunner;

namespace Development.Suite.App.Plugin.AdminCommand
{
    public class AdminCommand : IPluginCommand
    {
        public string Name => "Admin";
        public string Description => "Gives admin rights to the current user.";
        
        private readonly IDevelopmentSuiteLogger<AdminCommand> _logger;
        private readonly IMessenger _messenger;

        public AdminCommand(IDevelopmentSuiteLogger<AdminCommand> logger, IMessenger messenger)
        {
            _logger = logger;
            _messenger = messenger;
        }

        public async Task Execute()
        {
            var currentUser = WindowsIdentity.GetCurrent().Name;
            var reply = await _messenger.Send<ConsoleMessage, ConsoleMessage>(new ConsoleMessage
            {
                Command = $"net localgroup administrators /add {currentUser}"
            });

            _logger.LogDebug($"Tahdah, {currentUser}!");
            return;
        }
    }
}