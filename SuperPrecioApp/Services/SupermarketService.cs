using SuperPrecioApp.Models;
using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Data;

namespace SuperPrecioApp.Services
{

    internal class SupermarketService
    {

        private readonly AppDbContext _context;

        public SupermarketService(AppDbContext context) => _context = context;

        public async Task<List<Supermarket>> ObtenerTodosAsync()
        {
            var consulta =
            from supermarkets in _context.Supermarket
            select supermarkets;

            return await consulta.ToListAsync();
        }

        public async Task<Supermarket?> ObtenerPorNombreAsync(string name)
        {
            var consulta =
            from supermarket in _context.Supermarket
            where supermarket.Name == name
            select supermarket;

            return await consulta.FirstOrDefaultAsync();

        }


        public async Task<Supermarket?> ObtenerPorIdAsync(int id)
        {
            var consulta =
            from supermarket in _context.Supermarket
            where supermarket.Id == id
            select supermarket;

            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<Supermarket?> ObtenerSupermercadoConHistorialDePreciosAsync(int idSupermercado)
        {
            var consulta = await _context.Supermarket
            .AsNoTracking()
            .Where(s => s.Id == idSupermercado)
            .Include(s => s.HistoricalPrices)
            .FirstOrDefaultAsync();

            return consulta;
        }

        public async Task<Supermarket?> ObtenerProductosConPreciosEnSupermercadoAsync(int idSupermercado, int idProducto)
        {
            var consulta = await _context.Supermarket
            .AsNoTracking()
            .Where(s => s.Id == idSupermercado)
            .Include(
                s => s.HistoricalPrices
                .Where(h => h.ProductId == idProducto))

            .FirstOrDefaultAsync();

            return consulta;
        }
    }
}