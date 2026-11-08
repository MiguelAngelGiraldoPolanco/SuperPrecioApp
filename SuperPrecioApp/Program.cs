using SuperPrecioApp.Data;

// Comprobar si se ha solicitado un reinicio de la base de datos.
if (args.Contains("--reset-database", StringComparer.OrdinalIgnoreCase))
{
    Console.WriteLine("¡ATENCIÓN! Se eliminarán todos los datos.");
    Console.Write("¿Quieres continuar? (S/N): ");

    string? confirmation = Console.ReadLine();

    if (string.Equals(confirmation, "S", StringComparison.OrdinalIgnoreCase))
    {
        await DatabaseInitializer.ResetAsync();

        Console.WriteLine("Base de datos reiniciada correctamente.");
    }
    else
    {
        Console.WriteLine("Operación cancelada.");
    }
}
else
{
    await DatabaseInitializer.InitializeAsync();

    Console.WriteLine("Base de datos inicializada correctamente.");
}