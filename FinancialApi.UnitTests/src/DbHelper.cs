using FinancialApi.Config;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.UnitTests
{

    public static class DbHelper
    {
        public static DataBaseContext Connection()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
               .UseSqlServer("Data Source=127.0.0.1,1433;Initial Catalog=Financial;User ID=sa;Password=@A1b2 C3d4")
               .Options;

            var context = new DataBaseContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

    }
}