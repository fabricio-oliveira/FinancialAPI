using System;
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

            Console.WriteLine("XXXXXXXXXXXXXXX Open Conection XXXXXXXXXXXXXXX");

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


        //public static void Cleaner()
        //{
        //    if (_context == null)
        //        throw new Exception("Database Context not Initialize");

        //    try
        //    {
        //        _context.Database.RollbackTransaction();
        //    }
        //    catch (InvalidOperationException)
        //    {

        //    }


        //    _context.Database.BeginTransaction();
        //}

    }
}