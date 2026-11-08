using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Models
{
    internal class Product
    {
        public int Id { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Group { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;
    }
}