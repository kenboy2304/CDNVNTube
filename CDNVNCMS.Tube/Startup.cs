using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CDNVNCMS.Tube.Startup))]
namespace CDNVNCMS.Tube
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
