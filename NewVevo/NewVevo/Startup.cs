using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewVevo.Startup))]
namespace NewVevo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
