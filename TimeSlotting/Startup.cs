using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TimeSlotting.Startup))]
namespace TimeSlotting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
