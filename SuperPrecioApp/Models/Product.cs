using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Models
{
    internal record Product
    {
        public int Id { get; init; }
        public string Slug { get; init; } = string.Empty;           // Ej: aceite-oliva-virgen-extra
        public string Name { get; init; } = string.Empty;           // Ej: Aceite de oliva virgen extra
        public string Group { get; init; } = string.Empty;            // Ej: básicos
        public string Unit { get; init; } = string.Empty;            // Ej: L, kg

        // Lista / Array con todos los precios que ha tenido este producto a lo largo del tiempo
        public ICollection<HistoricalPrice> HistoricalPrices { get; init; } = [];
    }
}