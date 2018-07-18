using System;
using FinancialApi.Config;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.UnitTests
{

    public static class DatabaseHelper
    {
        static DataBaseContext _context = null;
        static SqliteConnection _connection = null;
       
       public static DataBaseContext Connection()
        {
            if (_context != null)
                return _context;

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new DataBaseContext(options);
            _context.Database.EnsureCreated();

            return _context;
        
        
        }
        public static void CleanData()
        {
           _connection.Close();
           _context = null;
        }

    }
}