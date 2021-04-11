using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Real_Estate_Project.Startup))]
namespace Real_Estate_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
