using SuperPrecioApp.Data;
using SuperPrecioApp.Services;

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
Console.WriteLine("¡Proceso finalizado con éxito!");