namespace SuperPrecioApp.Models
{
    internal record HistoricalPrice
    {
        public int Id {get; init; }
        public int ProductId { get; init; }
        public int SupermarketId { get; init; }
        public decimal Price { get; init; }
        public DateTime Date { get; init; }     // fecha en la que se registro el nuevo precio, no la fecha en la que se hizo el scrapeo
   
        // Propiedades de navegación (Permiten hacer consultas en C# tipo: precio.Producto.Nombre)
        public Product Product { get; init; } = null!;
        public Supermarket Supermarket { get; init; } = null!;
    }
}