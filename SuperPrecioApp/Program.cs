using SuperPrecioApp.Data;
using SuperPrecioApp.Services;
using SuperPrecioApp.Views;

Console.WriteLine("Iniciando importación directa...");
/*
 * Este código es un ejemplo de cómo podrías llamar al servicio de importación de datos desde la API.
 * Asegúrate de tener la base de datos configurada y que las migraciones estén aplicadas antes de ejecutar esto.
 * 
 * Nota: Este código se ejecuta en un contexto de consola simple. No se esta teniendo en cuenta llamadas  periodicas
 debe hacerse manualmente corriendo el programa.
 
// Creamos el contexto y el servicio a mano, sin frameworks pesados de fondo
using var context = new AppDbContext();
var importService = new ApiImportService(context);

// Llamamos a la API y guardamos
await importService.ImportarDatosCsvAsync();
*/
// EJEMPLO PARA LLAMAR A LOS METODOS Y PODER HACER LAS CONSULTAS
// PRIMERO: tenemos que darle contexto para que sepa donde esta la base de datos que va a consultar
using var context = new AppDbContext();

// SEGUNDO: se crea un objeto del servicio y se le pasa el contexto
var servicio = new ProductService(context);

// TERCERO: se guarda el resultado de la consulta en una variable  para poder oterar mostrar o lo que quieras//
var productos = await servicio.ObtenerTodosAsync();

foreach (var producto in productos)
{
Console.WriteLine($"{producto.Id} - {producto.Name}");
}

new MenuView().ShowMenu();


Console.WriteLine("¡Proceso finalizado con éxito!");