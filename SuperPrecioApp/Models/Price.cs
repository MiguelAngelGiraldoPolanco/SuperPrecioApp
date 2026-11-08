namespace SuperPrecioApp.Models
{
    internal class Price
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int SuperMarketId { get; set; }

        public SuperMarket SuperMarket { get; set; } = null!;

        public DateOnly Date { get; set; }

        public decimal PricePerUnit { get; set; }
    }
}