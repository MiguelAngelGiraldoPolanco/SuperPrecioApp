using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Models
{
    internal record Supermarket
    {
        public int Id {get; init; }
        public string Name { get; init; } = string.Empty;       // Ej: mercadona, dia
        
        // Lista / Array con todo el historial de precios que pertenecen a este supermercado
        public ICollection<HistoricalPrice> HistoricalPrices { get; init; } = [];
    }
}