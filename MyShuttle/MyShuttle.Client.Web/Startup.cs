using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyShuttle.Client.Web.Startup))]
namespace MyShuttle.Client.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
