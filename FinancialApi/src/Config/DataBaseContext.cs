using Microsoft.EntityFrameworkCore;
using System;
using FinancialApi.Models.DTO.Request;
using FinancialApi.src.Models.Entity;

namespace FinancialApi
{

    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
            
            var connectionDatabase = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
            if (connectionDatabase == null)
                throw new System.ArgumentException("DATABASE_CONNECTION cannot be null");

            optionsBuilder.UseSqlServer(@connectionDatabase);
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Input>();
            modelBuilder.Entity<Output>();
            modelBuilder.Entity<Charge>();

            modelBuilder.Entity<Account>();

            modelBuilder.Entity<CashFlow>();
                
        }

        //DataBase
        public DbSet<Account> Accounts { get; }

        public DbSet<CashFlow> CashFlows { get; }

        public DbSet<Input> Inputs { get; }
        public DbSet<Output> Outputs { get; }
        public DbSet<Charge> Charges { get; }
    }
}
