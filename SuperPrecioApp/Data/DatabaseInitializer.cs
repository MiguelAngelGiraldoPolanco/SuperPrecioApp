using Microsoft.EntityFrameworkCore;

namespace SuperPrecioApp.Data {
    internal static class DatabaseInitializer {
        // Decide al arrancar si reinicia o inicializa la base de datos.
        public static async Task RunStartupAsync(string[] args) {
            if (args.Contains("--reset-database", StringComparer.OrdinalIgnoreCase)) {
                Console.WriteLine("¡ATENCIÓN! Se eliminarán todos los datos.");
                Console.Write("¿Quieres continuar? (S/N): ");

                string? confirmation = Console.ReadLine();

                if (string.Equals(confirmation, "S", StringComparison.OrdinalIgnoreCase)) {
                    await ResetAsync();

                    Console.WriteLine("Base de datos reiniciada correctamente.");
                }
                else {
                    Console.WriteLine("Operación cancelada.");
                }
            }
            else {
                await InitializeAsync();

                Console.WriteLine("Base de datos inicializada correctamente.");
            }
        }

        // Inicialización normal: crea o actualiza la base de datos.
        public static async Task InitializeAsync() {
            using var db = new AppDbContext();

            await db.Database.MigrateAsync();
        }

        // Reinicio explícito: elimina y vuelve a crear la base de datos.
        public static async Task ResetAsync() {
            using var db = new AppDbContext();

            await db.Database.EnsureDeletedAsync();

            await db.Database.MigrateAsync();
        }
    }
}