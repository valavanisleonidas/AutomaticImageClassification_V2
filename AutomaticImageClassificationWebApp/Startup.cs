using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutomaticImageClassificationWebApp.Startup))]
namespace AutomaticImageClassificationWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
