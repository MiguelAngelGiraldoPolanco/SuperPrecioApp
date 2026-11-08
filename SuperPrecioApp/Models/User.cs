using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Models
{
    internal class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string HashPassword { get; set; } = string.Empty;
    }
}