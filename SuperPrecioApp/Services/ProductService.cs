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
            return await _context.Products.ToListAsync(); // ¡Aquí va el LINQ!
        }
    }
}