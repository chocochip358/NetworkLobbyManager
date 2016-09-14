using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameLobbyServer.Startup))]
namespace GameLobbyServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
