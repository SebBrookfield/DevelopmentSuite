using Development.Suite.Logging;

namespace Development.Suite.App.Plugin.AdminCommand
{
    public class GiveAdminCommand : BaseAdminCommand, IPluginCommand
    {
        public string Name => "Give Admin";
        public string Description => "Gives admin rights to the current user.";
        
        public GiveAdminCommand(IDevelopmentSuiteLogger<GiveAdminCommand> logger, IMessenger messenger) : base(logger, messenger)
        {
        }

        public async Task Execute()
        {
            await Execute("/add");
        }
    }
}