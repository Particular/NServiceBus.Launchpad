using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NsbBootstrapper.Startup))]
namespace NsbBootstrapper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
