using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MultiFileUpload.Startup))]
namespace MultiFileUpload
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
