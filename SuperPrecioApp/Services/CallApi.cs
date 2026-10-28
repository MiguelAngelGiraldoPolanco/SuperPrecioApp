using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Services
{
    // Clase que se utiliza para obtener los datos de precios de productos y supermercados desde una api.
    internal class CallApi
    {
        // Instanciamos a la calse HttpClient y se le añade la url de la api que vamos a llamar.
        private static HttpClient sharedClient = new()
        {
            // cesta-diaria.cvs.... Una fila por día, producto y cadena: el precio por kilo, litro o unidad.
            //fecha, producto, nombre, grupo, unidad, cadena, precio_por_unidad_eur

            BaseAddress = new Uri("https://tutandem.es"),
        };

        static async Task GetAsync(HttpClient httpClient)
        {
            using HttpResponseMessage response = await httpClient.GetAsync("datos/cesta-diaria.csv");

            response.EnsureSuccessStatusCode();
                

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
    }
}
