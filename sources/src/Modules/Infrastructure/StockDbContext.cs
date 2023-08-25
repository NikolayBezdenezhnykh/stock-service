using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class StockDbContext : DbContext
    {
        public DbSet<ReserveProduct> ReserveProducts { get; set; }

        public DbSet<AvailableProduct> AvailableProducts { get; set; }

        public StockDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StockDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
