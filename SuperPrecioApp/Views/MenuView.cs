using SuperPrecioApp.Data;
using SuperPrecioApp.Models;
using SuperPrecioApp.Services;

namespace SuperPrecioApp.Views {
    internal class MenuView {
        private static string pathTxt = BestPrices.getPathTxt();

        public MenuView() { }

        // Método para leer números enteros sin provocar excepciones
        private static int LeerEntero(string mensaje) {
            int numero;

            while (true) {
                Console.WriteLine(mensaje);

                // Verifica si lo que el cliente ha puesto es un int valido y si lo es retorna el numero
                if (int.TryParse(Console.ReadLine(), out numero)) {
                    return numero;
                }

                Console.WriteLine("Entrada no válida. Introduce un número entero.");
            }
        }

        public async Task MostrarMenuAsync() {
            int option;

            try {
                // pasamos el contexto de la base de datos
                using var context = new AppDbContext();

                // creamos objetos de los servicios pasandole el contexto de la base de datos a cada uno
                var sm = new SupermarketService(context);
                var p = new ProductService(context);
                var hp = new HistoricalPriceService(context);

                Console.WriteLine("\nBienvenido a SuperPrecioApp");
                do {
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("Selecciona una opción:");
                    Console.WriteLine("1. Listar productos");
                    Console.WriteLine("2. Listar supermercados");
                    Console.WriteLine("3. Consultar precios de un producto");
                    Console.WriteLine("4. Consultar historial de un producto en un supermercado");
                    Console.WriteLine("5. Para crear una lista de la compra");
                    Console.WriteLine("6. Para ver toda la lista de la compra");
                    Console.WriteLine("7. Para borrar la lista de la compra");
                    Console.WriteLine("0. Salir del programa");
                    Console.WriteLine("----------------------------------");

                    option = LeerEntero("Introduce una opción:");

                    // utilizamos un try catch en el menu por que existe algun caso que rompa el programa ,
                    // ejemplo que lo que se ingrese no sea un numero o que el numero este fuera de rango
                    try {
                        switch (option) {
                            case 1: {
                                    var productos = await p.ObtenerTodosAsync();

                                    // verificacion previa de que la lista que devuelve tenga objetos
                                    if (productos.Count == 0) {
                                        Console.WriteLine("No hay productos registrados.");
                                        break;
                                    }

                                    foreach (var producto in productos) {
                                        Console.WriteLine($"{producto.Id} - {producto.Name}");
                                    }

                                    break;
                                }

                            case 2: {
                                    var supermercados = await sm.ObtenerTodosAsync();

                                    if (supermercados.Count == 0) {
                                        Console.WriteLine("No hay supermercados registrados.");
                                        break;
                                    }

                                    foreach (var supermercado in supermercados) {
                                        Console.WriteLine($"{supermercado.Id} - {supermercado.Name}");
                                    }

                                    break;
                                }

                            case 3: {
                                    int idProducto = LeerEntero("Introduce el ID del producto:");

                                    if (idProducto <= 0) {
                                        Console.WriteLine("El ID debe ser mayor que cero.");
                                        break;
                                    }

                                    var producto = await p.ObtenerPorIdAsync(idProducto);

                                    if (producto == null) {
                                        Console.WriteLine("El producto no existe.");
                                        break;
                                    }

                                    var historial = await hp.ObtenerHistorialDePreciosPorProductoAsync(idProducto);

                                    if (historial.Count == 0) {
                                        Console.WriteLine("No hay historial de precios para este producto.");
                                        break;
                                    }

                                    Console.WriteLine($"Producto: {producto.Name}");

                                    foreach (var precio in historial) {
                                        Console.WriteLine($"{precio.Price} € - Supermercado ID: {precio.SupermarketId} - {precio.Date}");
                                    }

                                    break;
                                }

                            case 4: {
                                    int idProducto = LeerEntero("Introduce el ID del producto:");

                                    if (idProducto <= 0) {
                                        Console.WriteLine("El ID debe ser mayor que cero.");
                                        break;
                                    }

                                    int idSupermarket = LeerEntero("Introduce el ID del supermercado:");

                                    if (idSupermarket <= 0) {
                                        Console.WriteLine("El ID debe ser mayor que cero.");
                                        break;
                                    }

                                    var producto = await p.ObtenerPorIdAsync(idProducto);

                                    if (producto == null) {
                                        Console.WriteLine("El producto no existe.");
                                        break;
                                    }

                                    var supermercado = await sm.ObtenerPorIdAsync(idSupermarket);

                                    if (supermercado == null) {
                                        Console.WriteLine("El supermercado no existe.");
                                        break;
                                    }

                                    var historial = await hp.ObtenerHistorialDePreciosPorProductoYSupermercadoAsync(
                                        idProducto, idSupermarket);

                                    if (historial.Count == 0) {
                                        Console.WriteLine("No hay historial de precios para esta combinación.");
                                        break;
                                    }

                                    Console.WriteLine($"Producto: {producto.Name}");
                                    Console.WriteLine($"Supermercado: {supermercado.Name}");

                                    foreach (var precio in historial) {
                                        Console.WriteLine($"{precio.Price} € - {precio.Date}");
                                    }

                                    break;
                                }

                            case 5: {
                                    CreateBuyList();
                                    break;
                                }

                            case 6: {
                                    string contenido = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), pathTxt));
                                    Console.WriteLine(contenido);
                                    break;
                                }

                            case 7: {
                                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), pathTxt);
                                    if (File.Exists(filePath)) {
                                        File.Delete(filePath);
                                        Console.WriteLine("Lista de la compra eliminada.");
                                    }
                                    else {
                                        Console.WriteLine("No hay lista de la compra para eliminar.");
                                    }
                                    break;
                                }

                            case 0: {
                                    Console.WriteLine("Saliendo del programa...");
                                    break;
                                }

                            default: {
                                    Console.WriteLine("Opción no válida.");
                                    break;
                                }
                        }
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Se ha producido un error al ejecutar la operación.");
                        Console.WriteLine($"Detalle: {ex.Message}");
                    }

                } while (option != 0);
            }
            catch (Exception ex) {
                Console.WriteLine("No se ha podido iniciar o mantener la aplicación.");
                Console.WriteLine($"Detalle: {ex.Message}");
            }
        } // fin de MostrarMenuAsync

        // Pide nombres de productos y guarda en un txt el mejor precio de cada uno
        private static void CreateBuyList() {
            Console.WriteLine("Escribe el nombre EXACTO del producto que quieras. Escribe 'salir' para finalizar tu lista de la compra.");

            while (true) {
                string? productName = Console.ReadLine();

                if (productName is null || productName.Trim().ToLower() == "salir") {
                    break;
                }

                if (string.IsNullOrWhiteSpace(productName)) {
                    Console.WriteLine("Escriba el nombre del producto.");
                    continue;
                }

                HistoricalPrice? productSearch = BestPrices.GetBestPrice(productName.Trim());

                if (productSearch is null) {
                    Console.WriteLine("Producto no encontrado.");
                    continue;
                }

                string productInfo = $"{productSearch.Product.Name} - {productSearch.Price}€ - {productSearch.Supermarket.Name}";
                BestPrices.InsertIntoTxt(productInfo);
                Console.WriteLine("Producto agregado. Escribe otro producto o 'salir' para finalizar.");
            }
        } 
    } 
}