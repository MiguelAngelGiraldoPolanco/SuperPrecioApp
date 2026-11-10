using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Models
{
    internal record User
    {
        public int Id {get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string HashPassword { get; init; } = string.Empty;
   
    }
}