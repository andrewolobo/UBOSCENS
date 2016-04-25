using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UBOSCENS.Startup))]
namespace UBOSCENS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
