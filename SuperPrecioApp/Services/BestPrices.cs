using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Data;
using SuperPrecioApp.Models;


namespace SuperPrecioApp.Services {
    internal class BestPrices {
        private const string PATH_TXT = "lista_compra.txt";

        internal static string getPathTxt() {
            return PATH_TXT;
        }


        // funcion para crear la lista de la compra
        internal static void InsertIntoTxt(String productInfo) {
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), PATH_TXT);
            File.AppendAllLines(ruta, new[] { productInfo });   // AppendAllLines pone el salto de línea
        }

        // funcion para ver todos los productos por supermercado
        internal static List<HistoricalPrice> GetLowestPrices() {
            // se meten todos los productos que existen en la bbdd
            List<HistoricalPrice> productSearch = CreateProducts("");
            List<HistoricalPrice> lowestPrices = new();
            List<HistoricalPrice> toSuperMarket = new();

            // se reccorre el arraylist
            for (int i = 0; i < productSearch.Count; i++) {
                // metemos el primero en el arraylist, ya que de momento es el mas barato
                HistoricalPrice actual = productSearch[i];
                int posicion = -1;

                // se compara con el siguiente, ademmas con el if sabe si ya existe en el arraylist o no, y si existe, se compara el precio
                // es importante el id ya que el nombre puede ser igual, pero el id es unico
                for (int j = 0; j < lowestPrices.Count; j++) {
                    if (lowestPrices[j].Product.Id == actual.Product.Id) {
                        posicion = j;
                        break;
                    }
                }

                if (posicion == -1) {
                    lowestPrices.Add(actual);
                } else if (actual.Price < lowestPrices[posicion].Price) {
                    lowestPrices[posicion] = actual;
                }
            }

            // si es el producto mas barato, se mete en el arraylist de los productos mas baratos
            for (int i = 0; i < lowestPrices.Count; i++) {
                toSuperMarket.Add(lowestPrices[i]);
            }

            return toSuperMarket;
        }

        // consigue el producto de menor precio 
        public static HistoricalPrice? GetBestPrice(String ProductName) {
            List<HistoricalPrice> prices = CreateProducts(ProductName);

            if (prices.Count == 0) {
                return null;
            }

            HistoricalPrice lowestPrice = prices[0];

            for (int i = 1; i < prices.Count; i++) {
                if (prices[i].Price < lowestPrice.Price) {
                    lowestPrice = prices[i];
                }
            }

            return lowestPrice;

        }


        // De momento, para probar, pilla los productos desde el txt
        // Cuando la BBDD este lista se cambia a la consulta 
        private static List<HistoricalPrice> CreateProducts(string? nombreProducto = null) {
            nombreProducto = string.IsNullOrWhiteSpace(nombreProducto) ? null : nombreProducto.Trim();

            using var context = new AppDbContext();

            // Para cada producto + supermercado, solo el precio de la fecha más reciente
            var consulta = context.HistoricalPrices
                .AsNoTracking()
                .Include(h => h.Product)
                .Include(h => h.Supermarket)
                .Where(h => h.Date == context.HistoricalPrices
                    .Where(x => x.ProductId == h.ProductId && x.SupermarketId == h.SupermarketId)
                    .Max(x => x.Date));

            // Si se busca un producto concreto (LIKE en SQLite no distingue mayúsculas)
            if (nombreProducto != null)
                consulta = consulta.Where(h => EF.Functions.Like(h.Product.Name, nombreProducto));

            return consulta.ToList();
        }


        


    }
}
