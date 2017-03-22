using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(softAdmin.Startup))]
namespace softAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
