using SuperPrecioApp.Models;
using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Data;

namespace SuperPrecioApp.Services
{

    //clase que llevas las consultas con metodos empleados para hacer las consultas a la base de datos usando linq 
    internal class HistoricalPriceService
    {

        private readonly AppDbContext _context;

        public HistoricalPriceService(AppDbContext context) => _context = context;

        public async Task<List<HistoricalPrice>> ObtenerHistorialDePreciosPorProductoAsync(int idProducto)
        {
            var historial = await (
            from price in _context.HistoricalPrices
            where price.ProductId == idProducto
            orderby price.Date descending
            select price
        ).ToListAsync();

            return historial
                .GroupBy(p => p.SupermarketId)
                .Select(g => g.First())
                .ToList();
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