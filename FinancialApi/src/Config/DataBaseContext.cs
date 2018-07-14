using Microsoft.EntityFrameworkCore;
using FinancialApi.Models.Entity;

namespace FinancialApi.Config
{

    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

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
            modelBuilder.Entity<Balance>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Interest>();

        }

        //DataBase
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Interest> Interests { get; set; }
    }
}
