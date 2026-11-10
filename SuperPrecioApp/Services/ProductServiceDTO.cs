using CsvHelper.Configuration.Attributes;

namespace SuperPrecioApp.Models;

// esta clase mapea todo lo que nos devuelve la api para que se pueda deserializar correctamente y crear
// los modelos
public class CsvProductoDto
{
    [Name("fecha")]
    public DateTime Fecha { get; set; }

    [Name("producto")]
    public string SlugProducto { get; set; } = string.Empty; // Ej: aceite-oliva-virgen-extra

    [Name("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Name("grupo")]
    public string Grupo { get; set; } = string.Empty;

    [Name("unidad")]
    public string Unidad { get; set; } = string.Empty; // Ej: L, kg, ud

    [Name("cadena")]
    public string Supermercado { get; set; } = string.Empty; // Ej: mercadona, dia

    [Name("precio_por_unidad_eur")]
    public decimal Precio { get; set; }
}