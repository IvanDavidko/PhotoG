using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoG.UI.Startup))]
namespace PhotoG.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
