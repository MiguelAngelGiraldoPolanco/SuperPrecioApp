using Microsoft.EntityFrameworkCore;
using SuperPrecioApp.Data;
using SuperPrecioApp.Models;


namespace SuperPrecioApp.Services {
    internal class BestPrices {

        // funcion para crear la lista de la compra
        internal static void CreateBuyList() {
            // en este array meteremos los productos que el usuario vaya introduciendo
            List<HistoricalPrice> prices = new();
            Console.WriteLine("Escribe el nombre EXACTO del producto que quieras. Escribe salir para finalizar tu lista de la compra.");
            String? productName = "";

            // se leen nombres de productos hasta que el usuario escriba "salir"
            do {
                // se lee el nombre del producto que el usuario ha escrito
                productName = Console.ReadLine();

                if (productName == "") {
                    Console.WriteLine("Escriba el nombre del producto.");
                }

                // en caso de que no escriba nada o escriba salir
                if (productName is null || productName.Trim().ToLower() == "salir") {
                    break;
                }

                // si escribe en blanco (o solo espacios), no hace nada y vuelve a pedir
                if (string.IsNullOrWhiteSpace(productName)) {
                    continue;
                }

                //se mete en el arraylist el producto
                HistoricalPrice? productSearch = GetBestPrice(productName);

                if (productSearch is null) {
                    Console.WriteLine("Producto no encontrado.");
                }
                else {

                    prices.AddRange(productSearch);

                    for (int i = 0; i < prices.Count; i++) {
                        Console.WriteLine($"Producto {i + 1}: {prices[i].Product.Name} - {prices[i].Price} - {prices[i].Supermarket.Name}");
                    }

                    Console.WriteLine("Producto agregado. Escribe otro producto o 'salir' para finalizar.");
                }
            } while (productName?.ToLower() != "salir");

            WriteTxt(prices);
        }


        // funcion para todos los productos
        internal static void ShowProductsList() {
            List<HistoricalPrice> productSearch = CrearProductos("");
            List<HistoricalPrice> lowestPrices = new();
            List<HistoricalPrice> toSuperMarket = new();

            for (int i = 0; i < productSearch.Count; i++) {
                HistoricalPrice actual = productSearch[i];

                int posicion = -1;
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

            for (int i = 0; i < lowestPrices.Count; i++) {
                toSuperMarket.Add(lowestPrices[i]);
            }

            showBySuperMarket(toSuperMarket);
        }


        internal static void showBySuperMarket(List<HistoricalPrice> prices) {
            // ordenamos el arraylist por supermercado
            for (int i = 0; i < prices.Count - 1; i++) {
                for (int j = 0; j < prices.Count - 1 - i; j++) {
                    if (string.Compare(prices[j].Supermarket.Name, prices[j + 1].Supermarket.Name) > 0) {
                        HistoricalPrice aux = prices[j];
                        prices[j] = prices[j + 1];
                        prices[j + 1] = aux;
                    }
                }
            }

            String supermarketName = prices[0].Supermarket.Name;

            for (int i = 0; i < prices.Count ; i++) {
                if (i == 0 || prices[i].Supermarket.Name != prices[i - 1].Supermarket.Name) {
                    Console.WriteLine("--------------------");
                    Console.WriteLine(prices[i].Supermarket.Name.ToUpper());
                    Console.WriteLine("--------------------");
                }

                Console.WriteLine($"{i + 1} - {prices[i].Product.Name} - {prices[i].Price}");

            }


        }

        // funcion para escribir en el txt
        internal static void WriteTxt(List<HistoricalPrice> product) {
            // creacion del txt para la lista de la compra
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "lista_compra.txt");

            using (StreamWriter writer = new StreamWriter(filePath)) {
                for (int i = 0; i < product.Count; i++) {
                    writer.WriteLine($"{product[i].Product.Name} - {product[i].Price} - {product[i].Supermarket.Name}");
                }
            }

            Console.WriteLine($"Archivo creado en: {filePath}");

        }


        // consigue el producto de menor precio 
        internal static HistoricalPrice? GetBestPrice(String ProductName) {
            List<HistoricalPrice> prices = CrearProductos(ProductName);

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

        internal static List<HistoricalPrice> CrearProductos(string? nombreProducto = null) {
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
