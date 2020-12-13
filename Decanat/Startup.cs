using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Decanat.Startup))]
namespace Decanat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
