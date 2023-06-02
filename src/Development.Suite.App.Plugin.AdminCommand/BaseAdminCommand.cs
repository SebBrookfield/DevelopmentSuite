using System.Security.Principal;
using Development.Suite.Logging;
using Development.Suite.Plugin.ConsoleRunner;

namespace Development.Suite.App.Plugin.AdminCommand
{
    public class BaseAdminCommand
    {
        private readonly IDevelopmentSuiteLogger<BaseAdminCommand> _logger;
        private readonly IMessenger _messenger;

        public BaseAdminCommand(IDevelopmentSuiteLogger<BaseAdminCommand> logger, IMessenger messenger)
        {
            _logger = logger;
            _messenger = messenger;
        }

        public async Task Execute(string @switch)
        {
            var currentUser = WindowsIdentity.GetCurrent().Name;
            var reply = await _messenger.Send<ConsoleMessage, ConsoleMessage>(new ConsoleMessage
            {
                Command = $"net localgroup administrators {@switch} {currentUser}"
            });

            _logger.LogDebug($"Tahdah, {currentUser}!");
            return;
        }
    }
}