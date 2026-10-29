using SuperPrecioApp.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Services
{
    // Clase que se utiliza para obtener los datos de precios de productos y supermercados desde una api.
    internal static class CallApi
    {
        // Instanciamos a la calse HttpClient y se le añade la url de la api que vamos a llamar.
        private static HttpClient sharedClient = new()
        {
            // cesta-diaria.cvs.... Una fila por día, producto y cadena: el precio por kilo, litro o unidad.
            //fecha, producto, nombre, grupo, unidad, cadena, precio_por_unidad_eur

            BaseAddress = new Uri("https://tutandem.es"),
        };

        internal static async Task GetAsync()
        {
            Console.WriteLine("Ejecutandose. Espere unos segundos ....");
            using HttpResponseMessage response = await sharedClient.GetAsync("datos/cesta-diaria.csv");
            Console.WriteLine("Se completo con exito");

            response.EnsureSuccessStatusCode().WriteRequestToConsole(); ;


            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }

        
    }
}
