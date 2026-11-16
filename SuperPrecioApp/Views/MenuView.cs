using SuperPrecioApp.Models;
using SuperPrecioApp.Services;
using SuperPrecioApp.Data;

namespace SuperPrecioApp.Views
{
    internal class MenuView
    {
        public MenuView() { }

        // Método para leer números enteros sin provocar excepciones
        private static int LeerEntero(string mensaje)
        {
            int numero;

            while (true)
            {
                Console.WriteLine(mensaje);

                // Verifica si lo que el cliente ha puesto es un int valido y si lo es retorna el numero
                if (int.TryParse(Console.ReadLine(), out numero))
                {
                    return numero;
                }

                Console.WriteLine("Entrada no válida. Introduce un número entero.");
            }
        }

        public async Task MostrarMenuAsync()
        {
			// cesta temporal con los productos que añade el usuario
			var cesta = new List<Product>();
			int option;

            try
            {
                // pasamos el contexto de la base de datos 
                using var context = new AppDbContext();

                // creamos objetos de los servicios pasandole el contexto de la base de datos a cada uno 
                var sm = new SupermarketService(context);
                var p = new ProductService(context);
                var hp = new HistoricalPriceService(context);

                Console.WriteLine("\nBienvenido a SuperPrecioApp");
                do
                {
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("Selecciona una opción:");
                    Console.WriteLine("1. Listar productos");
                    Console.WriteLine("2. Listar supermercados");
                    Console.WriteLine("3. Consultar precios de un producto");
                    Console.WriteLine("4. Consultar historial de un producto en un supermercado");
					Console.WriteLine("5. Buscar supermercado más barato");
					Console.WriteLine("6. Añadir a la cesta");
					Console.WriteLine("7. Ver la lista de la compra");
					Console.WriteLine("8. Eliminar producto de la lista de la compra");
					Console.WriteLine("9. Comparar cesta completa");
					Console.WriteLine("0. Salir del programa");
                    Console.WriteLine("----------------------------------");

                    option = LeerEntero("Introduce una opción:");

                    // utilizamos un try catch en el menu por que existe algun caso que rompa el programa , 
                    // ejemplo que lo que se ingrese no sea un numero o que el numero este fuera de rango 
                    try
                    {
                        switch (option)
                        {
                            case 1:
                                {
                                    var productos = await p.ObtenerTodosAsync();

                                    // verificacion previa de que la lista que devuelve tenga objetos
                                    if (productos.Count == 0)
                                    {
                                        Console.WriteLine("No hay productos registrados.");
                                        break;
                                    }

                                    foreach (var producto in productos)
                                    {
                                        Console.WriteLine($"{producto.Id} - {producto.Name}");
                                    }

                                    break;
                                }

                            case 2:
                                {
                                    var supermercados = await sm.ObtenerTodosAsync();

                                    if (supermercados.Count == 0)
                                    {
                                        Console.WriteLine("No hay supermercados registrados.");
                                        break;
                                    }

                                    foreach (var supermercado in supermercados)
                                    {
                                        Console.WriteLine($"{supermercado.Id} - {supermercado.Name}");
                                    }

                                    break;
                                }

                            case 3:
                                {
                                    int idProducto = LeerEntero("Introduce el ID del producto:");

                                    if (idProducto <= 0)
                                    {
                                        Console.WriteLine("El ID debe ser mayor que cero.");
                                        break;
                                    }

                                    var producto = await p.ObtenerPorIdAsync(idProducto);

                                    if (producto == null)
                                    {
                                        Console.WriteLine("El producto no existe.");
                                        break;
                                    }

                                    var historial = await hp.ObtenerHistorialDePreciosPorProductoAsync(idProducto);

                                    if (historial.Count == 0)
                                    {
                                        Console.WriteLine("No hay historial de precios para este producto.");
                                        break;
                                    }

                                    Console.WriteLine($"Producto: {producto.Name}");

                                    foreach (var precio in historial)
                                    {
                                        Console.WriteLine($"{precio.Price} € - Supermercado ID: {precio.SupermarketId} - {precio.Date}");
                                    }

                                    break;
                                }

                            case 4:
                                {
                                    int idProducto = LeerEntero("Introduce el ID del producto:");

                                    if (idProducto <= 0)
                                    {
                                        Console.WriteLine("El ID debe ser mayor que cero.");
                                        break;
                                    }

                                    int idSupermarket = LeerEntero("Introduce el ID del supermercado:");

                                    if (idSupermarket <= 0)
                                    {
                                        Console.WriteLine("El ID debe ser mayor que cero.");
                                        break;
                                    }

                                    var producto = await p.ObtenerPorIdAsync(idProducto);

                                    if (producto == null)
                                    {
                                        Console.WriteLine("El producto no existe.");
                                        break;
                                    }

                                    var supermercado = await sm.ObtenerPorIdAsync(idSupermarket);

                                    if (supermercado == null)
                                    {
                                        Console.WriteLine("El supermercado no existe.");
                                        break;
                                    }

                                    var historial = await hp.ObtenerHistorialDePreciosPorProductoYSupermercadoAsync(
                                        idProducto, idSupermarket);

                                    if (historial.Count == 0)
                                    {
                                        Console.WriteLine("No hay historial de precios para esta combinación.");
                                        break;
                                    }

                                    Console.WriteLine($"Producto: {producto.Name}");
                                    Console.WriteLine($"Supermercado: {supermercado.Name}");

                                    foreach (var precio in historial)
                                    {
                                        Console.WriteLine($"{precio.Price} € - {precio.Date}");
                                    }

                                    break;
                                }

							case 5: 
                                {
								    await BuscarSupermercadoMasBaratoAsync(p, hp, sm);
								    break;
							    }

							case 6: 
                                {
								    await AnadirACestaAsync(p, cesta);
								    break;
							    }

							case 7: 
								{
									VerCesta(cesta);
									break;
								}

							case 8: 
								{
									EliminarDeCesta(cesta);
									break;
								}

							case 9: 
                                {
								    await CompararCestaAsync(cesta, sm, hp);
								    break;
							    }


							case 0:
                                {
                                    Console.WriteLine("Saliendo del programa...");
                                    break;
                                }

                            default:
                                {
                                    Console.WriteLine("Opción no válida.");
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Se ha producido un error al ejecutar la operación.");
                        Console.WriteLine($"Detalle: {ex.Message}");
                    }

                } while (option != 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se ha podido iniciar o mantener la aplicación.");
                Console.WriteLine($"Detalle: {ex.Message}");
            }
        }

		// Busca el supermercado más barato para un producto
		private static async Task BuscarSupermercadoMasBaratoAsync(
			ProductService productService,
			HistoricalPriceService priceService,
			SupermarketService supermarketService) {

			// ID del producto que introduce el usuario
			int productId = LeerEntero("Introduce el ID del producto:");

			// se comprueba que el ID sea válido
			if (productId <= 0) {
				Console.WriteLine("El ID debe ser mayor que cero.");
				return;
			}

			// producto buscado en la base de datos
			var product = await productService.ObtenerPorIdAsync(productId);

			// se comprueba que el producto exista
			if (product == null) {
				Console.WriteLine("El producto no existe.");
				return;
			}

			// último precio del producto en cada supermercado
			var prices = await priceService.ObtenerHistorialDePreciosPorProductoAsync(productId);

			// se comprueba que haya algún precio registrado
			if (prices.Count == 0) {
				Console.WriteLine("No hay precios para este producto.");
				return;
			}

			// se parte del primer precio como el más barato
			var barato = prices[0];

			// se recorren los precios buscando uno más bajo
			foreach (var price in prices) {
				// si el precio es menor pasa a ser el más barato
				if (price.Price < barato.Price) {
					barato = price;
				}
			}

			// supermercado que tiene el precio más barato
			var supermarket = await supermarketService
				.ObtenerPorIdAsync(barato.SupermarketId);

			Console.WriteLine($"Producto: {product.Name}");
			Console.WriteLine($"Supermercado más barato: {supermarket?.Name}");
			Console.WriteLine($"Precio: {barato.Price} €");
		}

		// Añade un producto a la cesta temporal
		private static async Task AnadirACestaAsync(
			ProductService productService,
			List<Product> cesta) {

			Console.WriteLine("Introduce el ID o el nombre exacto del producto:");

			string? input = Console.ReadLine();

			// se comprueba que no esté vacío
			if (string.IsNullOrWhiteSpace(input)) {
				Console.WriteLine("No has escrito nada.");
				return;
			}

			// producto encontrado (puede ser nulo si no existe)
			Product? product;

			// si es un número se busca por ID, si no por nombre
			if (int.TryParse(input, out int productId)) {
				product = await productService.ObtenerPorIdAsync(productId);
			}
			else {
				product = await productService.ObtenerPorNombreAsync(input.Trim());
			}

			// se comprueba que el producto exista
			if (product == null) {
				Console.WriteLine("El producto no existe.");
				return;
			}

			// se recorre la cesta para evitar productos repetidos
			foreach (var item in cesta) {
				// si ya está en la cesta no se vuelve a añadir
				if (item.Id == product.Id) {
					Console.WriteLine("Ese producto ya está en la cesta.");
					return;
				}
			}

			// se guarda el producto en la cesta
			cesta.Add(product);

			Console.WriteLine($"Añadido: {product.Name}");
			Console.WriteLine($"Productos en la cesta: {cesta.Count}");
		}

		// Muestra por consola los productos de la lista de la compra
		private static void VerCesta(List<Product> cesta) {

			// se comprueba que la cesta tenga productos
			if (cesta.Count == 0) {
				Console.WriteLine("La cesta está vacía.");
				return;
			}

			Console.WriteLine("Lista de la compra:");

			// se muestra cada producto con su ID y su nombre
			foreach (var item in cesta) {
				Console.WriteLine($"{item.Id} - {item.Name}");
			}

			Console.WriteLine($"Total de productos: {cesta.Count}");
		}

		// Elimina un producto de la lista de la compra
		private static void EliminarDeCesta(List<Product> cesta) {

			// se comprueba que la cesta tenga productos
			if (cesta.Count == 0) {
				Console.WriteLine("La cesta está vacía.");
				return;
			}

			// se muestra la cesta para ver los IDs
			VerCesta(cesta);

			// ID del producto que quiere eliminar el usuario
			int productId = LeerEntero("ID del producto a eliminar:");

			// posición del producto en la cesta (-1 si no está)
			int position = -1;

			// se recorre la cesta buscando el producto por su ID
			for (int i = 0; i < cesta.Count; i++) {
				// si coincide el ID se guarda la posición y se sale
				if (cesta[i].Id == productId) {
					position = i;
					break;
				}
			}

			// se comprueba que el producto esté en la cesta
			if (position == -1) {
				Console.WriteLine("Ese producto no está en la cesta.");
				return;
			}

			// nombre del producto, guardado antes de eliminarlo
			string name = cesta[position].Name;

			// se elimina el producto de la cesta
			cesta.RemoveAt(position);

			Console.WriteLine($"Eliminado: {name}");
			Console.WriteLine($"Productos en la cesta: {cesta.Count}");
		}

		// Calcula el coste de la cesta en cada supermercado y muestra el ranking de menor a mayor.
		// Si a un supermercado le falta algún producto, se descarta del ranking
		private static async Task CompararCestaAsync(
			List<Product> cesta,
			SupermarketService supermarketService,
			HistoricalPriceService priceService) {

			// se comprueba que la cesta tenga productos
			if (cesta.Count == 0) {
				Console.WriteLine("La cesta está vacía.");
				return;
			}

			// todos los supermercados registrados
			var markets = await supermarketService.ObtenerTodosAsync();

			// coste total de la cesta por nombre de supermercado
			var totals = new Dictionary<string, decimal>();

			// se calcula el total de cada supermercado
			foreach (var market in markets) {
				// suma de los precios de la cesta en este supermercado
				decimal total = 0;

				// indica si el supermercado tiene todos los productos
				bool complete = true;

				// se busca el precio de cada producto de la cesta
				foreach (var item in cesta) {
					// precios del producto, el primero es el más reciente
					var prices = await priceService
						.ObtenerHistorialDePreciosPorProductoYSupermercadoAsync(
							item.Id, market.Id);

					// si falta el producto se descarta el supermercado
					if (prices.Count == 0) {
						complete = false;
						Console.WriteLine(
							$"Descartado {market.Name}: falta {item.Name}");
						break;
					}

					total = total + prices[0].Price;
				}

				// solo entran en el ranking los supermercados completos
				if (complete) {
					totals.Add(market.Name, total);
				}
			}

			// se comprueba que haya algún supermercado válido
			if (totals.Count == 0) {
				Console.WriteLine("Ningún supermercado tiene toda la cesta.");
				return;
			}

			// ranking ordenado del más barato al más caro
			var ranking = totals.OrderBy(t => t.Value);

			// posición actual dentro del ranking
			int position = 1;

			Console.WriteLine("Ranking de supermercados:");

			// se muestra cada supermercado con su total
			foreach (var entry in ranking) {
				Console.WriteLine($"{position}. {entry.Key}: {entry.Value} €");
				position++;
			}
		}

	}
}
