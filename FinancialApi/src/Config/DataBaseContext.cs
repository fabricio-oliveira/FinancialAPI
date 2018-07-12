using Microsoft.EntityFrameworkCore;
using System;
using FinancialApi.src.Models.Entity;

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
            
            modelBuilder.Entity<CashFlow>()
                        .HasMany(c => c.Inputs)
                        .WithOne(i => i.CashFlow)
                        .HasForeignKey(c => c.CashFlowId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashFlow>()
                        .HasMany(c => c.Outpus)
                        .WithOne(i => i.CashFlow)
                        .HasForeignKey(c => c.CashFlowId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashFlow>()
                        .HasMany(c => c.Charges)
                        .WithOne(i => i.CashFlow)
                        .HasForeignKey(c => c.CashFlowId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashFlow>()
                        .HasOne(c => c.Account)
                        .WithMany(a => a.CashFlows)
                        .HasForeignKey(c => c.AccountId)
                        .OnDelete(DeleteBehavior.Restrict);
                
        }

        //DataBase
        public DbSet<Account> Accounts { get; set; }

        public DbSet<CashFlow> CashFlows { get; set; }

        public DbSet<Input> Inputs { get; set;  }
        public DbSet<Output> Outputs { get; set; }
        public DbSet<Charge> Charges { get; set; }
    }
}
