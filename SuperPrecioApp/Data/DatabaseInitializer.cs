using Microsoft.EntityFrameworkCore;

namespace SuperPrecioApp.Data {
    internal static class DatabaseInitializer {
        public static async Task InitializeAsync() {
            using var db = new AppDbContext();

            // 1. Obtener la ruta del archivo de la base de datos desde la conexión de EF Core
            var connectionString = db.Database.GetConnectionString();
            
            // extraemos la ruta del archivo y aseguramos que el directorio exista si es SQLite
            if (!string.IsNullOrEmpty(connectionString))
            {
                var builder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString);
                if (!string.IsNullOrEmpty(builder.DataSource))
                {
                    var dbPath = Path.GetFullPath(builder.DataSource);
                    var directory = Path.GetDirectoryName(dbPath);

                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                        Console.WriteLine($"Directorio de base de datos creado: {directory}");
                    }
                }
            }

            // 2. Aplicar migraciones normalmente (crea la BD si no existe)
            await db.Database.MigrateAsync();
            Console.WriteLine("Base de datos inicializada correctamente.");
        }
    }
}