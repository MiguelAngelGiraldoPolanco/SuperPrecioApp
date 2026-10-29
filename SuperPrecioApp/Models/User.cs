using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPrecioApp.Models
{
    internal record User(int Id, string Name, string Email, string HashPassword);
}
