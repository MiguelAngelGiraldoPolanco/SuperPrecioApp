using SuperPrecioApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace SuperPrecioApp.Services {
    internal class TxtBestPrices {

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

        // consigue el mejor producto de menor precio 
        internal static Price? GetBestPrice(String NombreProducto) {
            List<Price> prices = CrearProductos(NombreProducto);

            if (prices.Count == 0) {
                return null;
            }

            Price precioMenor = prices[0];

            for (int i = 1; i < prices.Count; i++) {
                if (prices[i].PricePerUnit < precioMenor.PricePerUnit) {
                    precioMenor = prices[i];
                }
            }

            return precioMenor;

        }


        // De momento, para probar, pilla los productos desde el txt
        // Cuando la BBDD este lista se cambia a la consulta 

        // devuelve TODOS los productos que coincidan con el nombre, no solo el mejor precio
        internal static List<Price> CrearProductos(String NombreProducto) {
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

                if (!DateOnly.TryParseExact(campos[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly fecha))
                    continue;
                if (!decimal.TryParse(campos[6], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio))
                    continue;

                string codigoProducto = campos[1];
                string nombre = campos[2];
                string grupo = campos[3];
                string unidad = campos[4];
                string cadena = campos[5];

                if (NombreProducto == nombre) {

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
