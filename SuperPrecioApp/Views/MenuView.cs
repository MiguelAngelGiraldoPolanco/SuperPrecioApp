using SuperPrecioApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Views
{
    internal class MenuView
    {
        public void ShowMenu() {

            Console.WriteLine("Bienvenido a SuperPrecioApp");
            string? option;

            do {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Selecciona una opción:");
                Console.WriteLine("1. Hacer la lista de la compra");
                Console.WriteLine("2. Ver todos los productos");
                Console.WriteLine("0. Salir del programa");
                Console.WriteLine("----------------------------------------");

                option = Console.ReadLine();

                if (option == "1") {
                    CreateTxtBestPrices();
                }

                else if (option == "2") {
                    ShowAllProducts();
                }

                else if (option != "0" && option is not null) {
                    Console.WriteLine("Opción no válida.");
                }

            } while (option != "0" && option is not null);
        }

        public static void CreateTxtBestPrices() {
            BestPrices.CreateBuyList();
        }

        public static void ShowAllProducts() {
            BestPrices.ShowProductsList();
        }
    }
}
