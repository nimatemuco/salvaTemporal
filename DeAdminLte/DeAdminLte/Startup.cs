using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeAdminLte.Startup))]
namespace DeAdminLte
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
