using SuperPrecioApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Services {
    internal class TxtBestPrices {


       
        internal static void CreateBuyList() {
            // creacion del txt para la lista de la compra
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "lista_compra.txt");

            // en este array meteremos los productos que el usuario vaya introduciendo
            List<Product> products = new();

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
                products.Add(GetBestPrice(productName));
                Console.WriteLine("Producto agregado. Escribe otro producto o 'salir' para finalizar.");

            } while (productName?.ToLower() != "salir");

            // se escribe en el txt de todos los productos que el usuario ha introducido
            // mas adelante habria que escribir el precio y ordenar por supermercados
            File.WriteAllLines(filePath, products.Select(p => p.Name));


            Console.WriteLine($"Archivo creado en: {filePath}");
        }
        
        internal static Product GetBestPrice(String NombreProducto) {

            // de momento la bbdd esta vacia, para probar de momento se crea un producto de prueba
            // ademas, creo que habria que usar la clase Price, con el consiguiente cambio de clase
            Product test = new Product {
                Id = 1,
                ProductCode = "P001",
                Name = NombreProducto,
                Group = "General",
                Unit = "kg"
            };

            return test;
        }
        


    }
}
