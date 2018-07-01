using Microsoft.EntityFrameworkCore;
using System;
using FinancialApi.Model;

namespace FinancialApi
{

    public class DataBaseContext : DbContext
    {

        public DataBaseContext()
        { }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
            
            var connectionDatabase = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
            if (connectionDatabase == null)
                throw new System.ArgumentException("DATABASE_CONNECTION cannot be null");

            optionsBuilder.UseSqlServer(@connectionDatabase);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>()
                .HasDiscriminator<string>("Type")
                        .HasValue<Payment>("payment")
                        .HasValue<Receipt>("receipt");
        }

        //DataBase
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
    }
}
