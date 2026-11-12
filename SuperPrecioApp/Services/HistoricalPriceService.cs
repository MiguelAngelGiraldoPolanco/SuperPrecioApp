using SuperPrecioApp.Models;
using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Data;

namespace SuperPrecioApp.Services
{

    internal class HistoricalPriceService
    {

        private readonly AppDbContext _context;

        public HistoricalPriceService(AppDbContext context) => _context = context;

        public async Task<List<HistoricalPrice>> ObtenerHistorialDePreciosPorProductoAsync(int idProducto)
        {
            var consulta =
            from historicalPrices in _context.HistoricalPrices
            where historicalPrices.ProductId == idProducto
            orderby historicalPrices.Date descending
            select historicalPrices;

            return await consulta.ToListAsync();
        }

        public async Task<List<HistoricalPrice>> ObtenerHistorialDePreciosPorSupermercadoAsync(int idSupermercado)
        {
            var consulta =
            from historicalPrices in _context.HistoricalPrices
            where historicalPrices.SupermarketId == idSupermercado
            orderby historicalPrices.Date descending
            select historicalPrices;

            return await consulta.ToListAsync();
        }


        public async Task<List<HistoricalPrice>> ObtenerHistorialDePreciosPorProductoYSupermercadoAsync(int idProducto, int idSupermercado)
        {
            var consulta =
            from historicalPrices in _context.HistoricalPrices
            where historicalPrices.SupermarketId == idSupermercado && historicalPrices.ProductId == idProducto
            orderby historicalPrices.Date descending
            select historicalPrices;

            return await consulta.ToListAsync();
        }
    }
}