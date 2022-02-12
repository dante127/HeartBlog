using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HeartBlog.Startup))]
namespace HeartBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
