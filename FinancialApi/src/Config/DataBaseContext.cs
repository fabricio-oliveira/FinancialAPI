using Microsoft.EntityFrameworkCore;
using FinancialApi.Models.Entity;

namespace FinancialApi.Config
{

    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Entry>();

            modelBuilder.Entity<Balance>()
                        .HasOne(c => c.Account)
                        .WithMany(a => a.Balances)
                        .HasForeignKey(c => c.AccountId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>();

            modelBuilder.Entity<Interest>()
                        .HasOne(i => i.Account)
                        .WithMany(a => a.Interests)
                        .HasForeignKey(i => i.AccountId)
                        .OnDelete(DeleteBehavior.Restrict);

        }

        //DataBase
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Interest> Interests { get; set; }
    }
}
