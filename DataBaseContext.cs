using Microsoft.EntityFrameworkCore;
using System;
using Model;

namespace FinancialApi
{

     public class DataBaseContext : DbContext
     {

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
           var connectionDatabase = Environment.GetEnvironmentVariable("CONNECTION_STRING");
           Console.WriteLine($"testestes:{connectionDatabase}");
           if (connectionDatabase == null)
              throw new System.ArgumentException("CONNECTION_STRING cannot be null", "original");

           optionsBuilder.UseSqlServer(@connectionDatabase);
       }

       public DbSet<Pagamento> Pagamento { get; set; }
       public DbSet<Recebimento> Recebimento { get; set; }
       // public DbSet<FluxoDeCaixa> FluxoDeCaixa { get; set; }
     }
}
