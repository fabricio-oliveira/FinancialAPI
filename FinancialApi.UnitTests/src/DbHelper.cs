using FinancialApi.Config;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.UnitTests
{

    public static class DbHelper
    {
        static DataBaseContext _context = null;
        public static DataBaseContext Connection()
        {
            if (_context != null)
                return _context;

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseSqlite(connection)
                .Options;

            _context = new DataBaseContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            return _context;
        }

    }
}