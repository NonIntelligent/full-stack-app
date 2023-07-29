using System.Data.SqlClient;
using System.Data;

namespace M70Service.Data.Database
{
    // https://codedocu.com/Net-Framework/ASP_dot_Net-Core/Data-Model/Asp_dot_Net-Core_colon_-How-to-get-the-ApplicationDbContext-in-an-Asp_dot_Net-Core-MVC-application?2221
    public static class EF_Model
    {
        public static ApplicationDbContext dbContext = null;

        public static void Initialize_DbContext_Startup(IServiceProvider serviceProvider) {
            IServiceScope serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        }
    }
}
