using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Models;

namespace SuperPrecioApp.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Supermarket> Supermarkets => Set<Supermarket>();
        public DbSet<User> Users => Set<User>();
        public DbSet<HistoricalPrice> HistoricalPrices => Set<HistoricalPrice>();

        // --- CONSTRUCTOR 1: Vacío (Permite hacer "new AppDbContext()") ---
        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Detecta automáticamente si está en Docker (/app/data) o en local
            var dbPath = Directory.Exists("/app/data")
                ? Path.Combine("/app/data", "superprecio.db")
                //: Path.Combine(Directory.GetCurrentDirectory(), "superprecio.db");
                : Path.GetFullPath(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "..", "data", "superprecio.db"));

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ¡Clave! Evita duplicados exactos para el mismo producto, supermercado y fecha
            modelBuilder.Entity<HistoricalPrice>()
                .HasIndex(p => new
                {
                    p.ProductId,
                    p.SupermarketId,
                    p.Date
                })
                .IsUnique();
        }
    }
}