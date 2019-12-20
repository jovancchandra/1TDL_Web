using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_1TDL_Web.Startup))]
namespace _1TDL_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
