using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Models;

namespace SuperPrecioApp.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<SuperMarket> SuperMarkets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Price> Prices { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "SuperPrecio.db"
            );

            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Price>()
                .HasIndex(p => new
                {
                    p.ProductId,
                    p.SuperMarketId,
                    p.Date
                })
                .IsUnique();
        }
    }
}