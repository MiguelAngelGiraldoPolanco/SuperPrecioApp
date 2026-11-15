using SuperPrecioApp.Models;
using SuperPrecioApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SuperPrecioApp.Views
{
    internal class MenuView
    {
        private static string pathTxt = BestPrices.getPathTxt();
        public void ShowMenu() {

        Console.WriteLine("Bienvenido a SuperPrecioApp");
            string? option;

            do {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Selecciona una opción:");
                Console.WriteLine("1. Hacer la lista de la compra");
                Console.WriteLine("2. Ver todos los productos");
                Console.WriteLine("3. Ver la lista de la compra");
                Console.WriteLine("4. Eliminar la lista de la compra");
                Console.WriteLine("5. Eliminar un producto de la lista de la compra");
                Console.WriteLine("0. Salir del programa");
                Console.WriteLine("----------------------------------------");

                option = Console.ReadLine();

                if (option == "1") {
                    CreateTxtBestPrices();
                }

                else if (option == "2") {
                    ShowAllProducts();
                }

                else if (option == "3") {
                    ShowBuyList();
                }

                else if (option == "4") {
                    DeleteTxt();
                }

                else if (option == "5") {
                    Console.WriteLine("Escribe el nombre del producto que quieres eliminar:");
                    string? productToDelete = Console.ReadLine();
                    if (productToDelete is not null) {
                        DeleteProductFromTxt(productToDelete);
                    }
                }

                else if (option != "0" && option is not null) {
                    Console.WriteLine("Opción no válida.");
                }

            } while (option != "0" && option is not null);
        }

        public static void CreateTxtBestPrices() {
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


                // se busca el producto con el nombre que ha escrito el usuario
                HistoricalPrice? productSearch = BestPrices.GetBestPrice(productName);



                if (productSearch is null) {
                    Console.WriteLine("Producto no encontrado.");
                }
                else {
                    String productInfo = productSearch is not null ? $"{productSearch.Product.Name} - {productSearch.Price}e - {productSearch.Supermarket.Name}" : "Producto no encontrado.";
                    BestPrices.InsertIntoTxt(productInfo);
                    Console.WriteLine("Producto agregado. Escribe otro producto o 'salir' para finalizar.");
                }
            } while (productName?.ToLower() != "salir");
        }

        public static void ShowAllProducts() {
            List<HistoricalPrice> lowestPrices = BestPrices.GetLowestPrices();

            // ordenamos el arraylist por supermercado
            for (int i = 0; i < lowestPrices.Count - 1; i++) {
                for (int j = 0; j < lowestPrices.Count - 1 - i; j++) {
                    if (string.Compare(lowestPrices[j].Supermarket.Name, lowestPrices[j + 1].Supermarket.Name) > 0) {
                        HistoricalPrice aux = lowestPrices[j];
                        lowestPrices[j] = lowestPrices[j + 1];
                        lowestPrices[j + 1] = aux;
                    }
                }
            }

            String supermarketName = lowestPrices[0].Supermarket.Name;

            for (int i = 0; i < lowestPrices.Count; i++) {
                if (i == 0 || lowestPrices[i].Supermarket.Name != lowestPrices[i - 1].Supermarket.Name) {
                    Console.WriteLine("--------------------");
                    Console.WriteLine(lowestPrices[i].Supermarket.Name.ToUpper());
                    Console.WriteLine("--------------------");
                }

                Console.WriteLine($"{i + 1} - {lowestPrices[i].Product.Name} - {lowestPrices[i].Price}");
            }
        }

        public static void ShowBuyList() {
            string contenido = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), pathTxt));
            Console.WriteLine(contenido);
        }

        public static void DeleteTxt() {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), pathTxt);
            if (File.Exists(filePath)) {
                File.Delete(filePath);
                Console.WriteLine("Lista de la compra eliminada.");
            }
            else {
                Console.WriteLine("No hay lista de la compra para eliminar.");
            }
        }

        public static void DeleteProductFromTxt(string productName) {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), pathTxt);
            if (File.Exists(filePath)) {
                var lines = File.ReadAllLines(filePath).ToList();
                var lineToRemove = lines.FirstOrDefault(line => line.Contains(productName, StringComparison.OrdinalIgnoreCase));
                if (lineToRemove != null) {
                    lines.Remove(lineToRemove);
                    File.WriteAllLines(filePath, lines);
                    Console.WriteLine($"Producto '{productName}' eliminado de la lista de la compra.");
                }
                else {
                    Console.WriteLine($"Producto '{productName}' no encontrado en la lista de la compra.");
                }
            }
            else {
                Console.WriteLine("No hay lista de la compra para eliminar productos.");
            }
        }

    }
}
