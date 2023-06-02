using Development.Suite.Logging;

namespace Development.Suite.App.Plugin.AdminCommand
{
    public class RemoveAdminCommand : BaseAdminCommand, IPluginCommand
    {
        public string Name => "Remove Admin";
        public string Description => "Removes admin rights for the current user.";
        
        public RemoveAdminCommand(IDevelopmentSuiteLogger<GiveAdminCommand> logger, IMessenger messenger) : base(logger, messenger)
        {
        }

        public async Task Execute()
        {
            await Execute("/del");
        }
    }
}