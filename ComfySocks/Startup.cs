using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ComfySocks.Startup))]
namespace ComfySocks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
