
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Data;
using SuperPrecioApp.Models;

namespace SuperPrecioApp.Services
{
    internal class ApiImportService
    {
        private readonly AppDbContext _context;

        public ApiImportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportarDatosCsvAsync()
        {
            Console.WriteLine("Descargando datos de la API...");

            // Usamos tu cliente HTTP para descargar el CSV
            using var client = new HttpClient { BaseAddress = new Uri("https://tutandem.es") };
            using var response = await client.GetAsync("datos/cesta-diaria.csv");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error al descargar el CSV: {response.StatusCode}");
                return;
            }

            // Leemos el stream del CSV
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);
            
            // Usamos tu CsvProductoDto directamente aquí
            var registros = csv.GetRecords<CsvProductoDto>();

            Console.WriteLine("Procesando e insertando registros en la base de datos...");

            foreach (var row in registros)
            {
                // 1. Buscar o crear el Supermercado (Cadena)
                var supermarket = await _context.Supermarkets
                    .FirstOrDefaultAsync(s => s.Name == row.Supermercado);

                if (supermarket == null)
                {
                    supermarket = new Supermarket { Name = row.Supermercado };
                    _context.Supermarkets.Add(supermarket);
                    await _context.SaveChangesAsync(); // Guardamos para obtener su ID autogenerado
                }

                // 2. Buscar o crear el Producto usando tu SlugProducto
                var producto = await _context.Products
                    .FirstOrDefaultAsync(p => p.Slug == row.SlugProducto); // O la propiedad que uses para el código/slug

                if (producto == null)
                {
                    producto = new Product
                    {
                        Slug = row.SlugProducto,
                        Name = row.Nombre,
                        Group = row.Grupo,
                        Unit = row.Unidad
                    };
                    _context.Products.Add(producto);
                    await _context.SaveChangesAsync();
                }

                // 3. Registrar el Precio (el índice único en el DbContext evitará duplicados)
                var existePrecio = await _context.HistoricalPrices
                    .AnyAsync(p => p.ProductId == producto.Id && 
                                   p.SupermarketId == supermarket.Id && 
                                   p.Date == row.Fecha);

                if (!existePrecio)
                {
                    var precioHistorial = new HistoricalPrice
                    {
                        ProductId = producto.Id,
                        SupermarketId = supermarket.Id,
                        Price = row.Precio,
                        Date = row.Fecha
                    };
                    _context.HistoricalPrices.Add(precioHistorial);
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("¡Importación de datos completada con éxito!");
        }
    }
}