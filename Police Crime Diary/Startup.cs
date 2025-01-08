using Microsoft.Owin;
using Owin;
using Police_Crime_Diary.Service;

[assembly: OwinStartupAttribute(typeof(Police_Crime_Diary.Startup))]
namespace Police_Crime_Diary
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            DatabaseConnection.OnDB();
            ConfigureAuth(app);
        }
    }
}
