using Microsoft.EntityFrameworkCore;
using System;
using FinancialApi.Models.Entity;

namespace FinancialApi.Config
{

    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options){

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Entry>()
                        .HasDiscriminator<string>("type")
                        .HasValue<Payment>("payment")
                        .HasValue<Receipt>("receipt");
            
            modelBuilder.Entity<Balance>()
                        .HasOne(c => c.Account)
                        .WithMany(a => a.Balances)
                        .HasForeignKey(c => c.AccountId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Balance>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Interest>();
                
        }

        //DataBase
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Entry> Entrys { get; set;  }
        public DbSet<Interest> Interests { get; set; }
    }
}
