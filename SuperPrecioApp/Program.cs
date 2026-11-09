using SuperPrecioApp.Data;

await DatabaseInitializer.RunStartupAsync(args);
new SuperPrecioApp.Views.MenuView().ShowMenu();