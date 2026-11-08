using Microsoft.EntityFrameworkCore;

namespace SuperPrecioApp.Data
{
    internal static class DatabaseInitializer
    {
        // Inicialización normal: crea o actualiza la base de datos.
        public static async Task InitializeAsync()
        {
            using var db = new AppDbContext();

            await db.Database.MigrateAsync();
        }

        // Reinicio explícito: elimina y vuelve a crear la base de datos.
        public static async Task ResetAsync()
        {
            using var db = new AppDbContext();

            await db.Database.EnsureDeletedAsync();

            await db.Database.MigrateAsync();
        }
    }
}