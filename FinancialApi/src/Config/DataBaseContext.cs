using Microsoft.EntityFrameworkCore;
using System;
using FinancialApi.Models.Entity;
using FinancialApi.src.Models.Entity;

namespace FinancialApi
{

    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

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
                        .HasValue<Payment>("pagamento")
                        .HasValue<Receipt>("recebimento");

            modelBuilder.Entity<ShortEntry>()
                .HasDiscriminator<string>("Type")
                        .HasValue<Input>("input")
                        .HasValue<Output>("output")
                        .HasValue<Charges>("charges");

        }

        //DataBase
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
    }
}
