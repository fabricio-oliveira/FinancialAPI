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

            modelBuilder.Entity<Balance>()
                        .HasMany(c => c.Inputs)
                        .WithOne(i => i.Balance)
                        .HasForeignKey(c => c.BalanceId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Balance>()
                        .HasMany(c => c.Outpus)
                        .WithOne(i => i.Balance)
                        .HasForeignKey(c => c.BalanceId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Balance>()
                        .HasMany(c => c.Charges)
                        .WithOne(i => i.Balance)
                        .HasForeignKey(c => c.BalanceId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Balance>()
                        .HasOne(c => c.Account)
                        .WithMany(a => a.Balances)
                        .HasForeignKey(c => c.AccountId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Balance>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Charge>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Input>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Output>().Property(p => p.RowVersion).IsRowVersion();

            //base.OnModelCreating(modelBuilder);
                
        }

        //DataBase
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Balance> Balances { get; set; }

        public DbSet<Input> Inputs { get; set;  }
        public DbSet<Output> Outputs { get; set; }
        public DbSet<Charge> Charges { get; set; }
    }
}
