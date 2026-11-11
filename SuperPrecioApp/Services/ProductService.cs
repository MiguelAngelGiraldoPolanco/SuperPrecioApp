using SuperPrecioApp.Models;
using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Data;


namespace SuperPrecioApp.Services
{
    internal class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context) => _context = context;

        public async Task<List<Product>> ObtenerTodosAsync()
        {
            var consulta =
            from products in _context.Products
            select products;

            return await consulta.ToListAsync();
        }

        public async Task<Product?> ObtenerPorNombreAsync(string name)
        {
            var consulta =
            from product in _context.Products
            where product.Name == name
            select product;

            return await consulta.FirstOrDefaultAsync();

        }

        public async Task<Product?> ObtenerPorIdAsync(int id)
        {
            var consulta =
            from product in _context.Products
            where product.Id == id
            select product;

            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<Product?> ObtenerProductoConHistorialDePreciosAsync(int idProducto)
        {
            var consulta = await _context.Products
            .Where(p => p.Id == idProducto)
            .Include(p => p.HistoricalPrices)
            .FirstOrDefaultAsync();

            return consulta;
        }

        public async Task<Product?> ObtenerPreciosDeProductoEnSupermercadoAsync(int idProducto, int idSupermercado)
        {
            var consulta = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == idProducto)
            .Include(
                p => p.HistoricalPrices
                .Where(h => h.SupermarketId == idSupermercado))

            .FirstOrDefaultAsync();

            return consulta;
        }
    }
}