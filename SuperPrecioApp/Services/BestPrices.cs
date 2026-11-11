using SuperPrecioApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace SuperPrecioApp.Services {
    internal class BestPrices {

        // funcion para crear la lista de la compra
        internal static void CreateBuyList() {
            // en este array meteremos los productos que el usuario vaya introduciendo
            List<Price> prices = new();
            Console.WriteLine("Escribe el nombre EXACTO del producto que quieras. Escribe salir para finalizar tu lista de la compra.");
            String? productName = "";

            // se leen nombres de productos hasta que el usuario escriba "salir"
            do {
                // se lee el nombre del producto que el usuario ha escrito
                productName = Console.ReadLine();

                // en caso de que no escriba nada o escriba salir
                if (productName is null || productName.ToLower() == "salir") {
                    break;
                }

                //se mete en el arraylist el producto
                Price? productSearch = GetBestPrice(productName);

                if (productSearch is null) {
                    Console.WriteLine("Producto no encontrado.");
                }
                else {

                    prices.AddRange(productSearch);

                    for (int i = 0; i < prices.Count; i++) {
                        Console.WriteLine($"Producto {i + 1}: {prices[i].Product.Name} - {prices[i].PricePerUnit} - {prices[i].SuperMarket.Name}");
                    }

                    Console.WriteLine("Producto agregado. Escribe otro producto o 'salir' para finalizar.");
                }
            } while (productName?.ToLower() != "salir");

            WriteTxt(prices);
        }


        // funcion para todos los productos
        internal static void ShowProductsList() {
            List<Price> productSearch = CrearProductos("");
            List<Price> lowestPrices = new();
            List<Price> toSuperMarket = new();

            for (int i = 0; i < productSearch.Count; i++) {
                Price actual = productSearch[i];

                int posicion = -1;
                for (int j = 0; j < lowestPrices.Count; j++) {
                    if (lowestPrices[j].Product.ProductCode == actual.Product.ProductCode) {
                        posicion = j;
                        break;
                    }
                }

                if (posicion == -1) {
                    lowestPrices.Add(actual);
                } else if (actual.PricePerUnit < lowestPrices[posicion].PricePerUnit) {
                    lowestPrices[posicion] = actual;
                }
            }

            for (int i = 0; i < lowestPrices.Count; i++) {
                toSuperMarket.Add(lowestPrices[i]);
            }

            showBySuperMarket(toSuperMarket);
        }


        internal static void showBySuperMarket(List<Price> prices) {
            // ordenamos el arraylist por supermercado
            for (int i = 0; i < prices.Count - 1; i++) {
                for (int j = 0; j < prices.Count - 1 - i; j++) {
                    if (string.Compare(prices[j].SuperMarket.Name, prices[j + 1].SuperMarket.Name) > 0) {
                        Price aux = prices[j];
                        prices[j] = prices[j + 1];
                        prices[j + 1] = aux;
                    }
                }
            }

            String supermarketName = prices[0].SuperMarket.Name;

            for (int i = 0; i < prices.Count ; i++) {
                if (i == 0 || prices[i].SuperMarket.Name != prices[i - 1].SuperMarket.Name) {
                    Console.WriteLine("--------------------");
                    Console.WriteLine(prices[i].SuperMarket.Name.ToUpper());
                    Console.WriteLine("--------------------");
                }

                Console.WriteLine($"{i + 1} - {prices[i].Product.Name} - {prices[i].PricePerUnit}");

            }


        }

        // funcion para escribir en el txt
        internal static void WriteTxt(List<Price> product) {
            // creacion del txt para la lista de la compra
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "lista_compra.txt");

            using (StreamWriter writer = new StreamWriter(filePath)) {
                for (int i = 0; i < product.Count; i++) {
                    writer.WriteLine($"{product[i].Product.Name} - {product[i].PricePerUnit} - {product[i].SuperMarket.Name}");
                }
            }

            Console.WriteLine($"Archivo creado en: {filePath}");

        }


        // consigue el producto de menor precio 
        internal static Price? GetBestPrice(String ProductName) {
            List<Price> prices = CrearProductos(ProductName);

            if (prices.Count == 0) {
                return null;
            }

            Price lowestPrice = prices[0];

            for (int i = 1; i < prices.Count; i++) {
                if (prices[i].PricePerUnit < lowestPrice.PricePerUnit) {
                    lowestPrice = prices[i];
                }
            }

            return lowestPrice;

        }

        /*
        // devuelve todos los productos, con su supermercados mas barato
        internal static List<Price> GetBestPrice() {
            List<Price> prices = CrearProductos();
            List<Price> productsReturn = new List<Price>();

            // ordena por nombre y, si el nombre es igual, por precio (de menor a mayor)
            prices = prices
                    .OrderBy(p => p.Product.Name, StringComparer.CurrentCultureIgnoreCase)
                    .ThenBy(p => p.PricePerUnit)
                    .ToList();


            for (int i = 0; i < prices.Count; i++) {
                // el primero de cada producto es el más barato (porque está ordenado por precio)
                if (i == 0 || prices[i].Product.Name != prices[i - 1].Product.Name) {
                    productsReturn.Add(prices[i]);
                }
            }


            return productsReturn;
        }
        */


        // De momento, para probar, pilla los productos desde el txt
        // Cuando la BBDD este lista se cambia a la consulta 

        internal static List<Price> CrearProductos(string? nombreProducto = null) {
            if (string.IsNullOrWhiteSpace(nombreProducto))
                nombreProducto = null;
            else
                nombreProducto = nombreProducto.Trim();

            string? rutaFichero = BuscarFichero("ejemploApi.txt");

            var precios = new List<Price>();



            if (rutaFichero is null) {
                Console.WriteLine("No se ha encontrado ejemploApi.txt.");
                return precios;
            }

            var productos = new Dictionary<string, Product>();
            var supermercados = new Dictionary<string, SuperMarket>();

            var urls = new Dictionary<string, string> {
                ["mercadona"] = "https://www.mercadona.es",
                ["dia"] = "https://www.dia.es",
                ["consum"] = "https://www.consum.es",
                ["bonarea"] = "https://www.bonarea.com",
                ["hiperdino"] = "https://www.hiperdino.es"
            };

            var lineas = File.ReadAllLines(rutaFichero, Encoding.UTF8);

            // Empezamos en 1 para saltar la cabecera
            for (int i = 1; i < lineas.Length; i++) {

                string linea = lineas[i].Trim();
                if (string.IsNullOrEmpty(linea))
                    continue;

                // fecha,producto,nombre,grupo,unidad,cadena,precio_por_unidad_eur
                string[] campos = linea.Split(',');
                if (campos.Length != 7)
                    continue; // línea mal formada

                string nombre = campos[2];

                // Si se busca un producto concreto, descartamos el resto
                if (nombreProducto != null &&
                    !string.Equals(nombre, nombreProducto, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                if (!DateOnly.TryParseExact(campos[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly fecha))
                    continue;
                if (!decimal.TryParse(campos[6], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio))
                    continue;

                string codigoProducto = campos[1];
                string grupo = campos[3];
                string unidad = campos[4];
                string cadena = campos[5];

                // Producto: se crea una sola vez y se reutiliza
                if (!productos.TryGetValue(codigoProducto, out Product? producto)) {
                    producto = new Product {
                        ProductCode = codigoProducto,
                        Name = nombre,
                        Group = grupo,
                        Unit = unidad
                    };
                    productos.Add(codigoProducto, producto);
                }

                // Supermercado: se crea una sola vez y se reutiliza
                if (!supermercados.TryGetValue(cadena, out SuperMarket? supermercado)) {
                    supermercado = new SuperMarket {
                        Name = cadena,
                        Url = urls.GetValueOrDefault(cadena, string.Empty)
                    };
                    supermercados.Add(cadena, supermercado);
                }

                precios.Add(new Price {
                    Product = producto,
                    SuperMarket = supermercado,
                    Date = fecha,
                    PricePerUnit = precio
                });
            }

            return precios;
        }

        private static string? BuscarFichero(string nombreFichero) {
            DirectoryInfo? carpeta = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (carpeta != null) {
                string ruta = Path.Combine(carpeta.FullName, nombreFichero);
                if (File.Exists(ruta)) {
                    return ruta;
                }
                carpeta = carpeta.Parent;
            }
            return null;
        }

    }
}
