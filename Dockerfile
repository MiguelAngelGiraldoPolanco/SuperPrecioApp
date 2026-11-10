# Etapa 1: Compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar todo el código fuente al contenedor
COPY . .

# Entrar a la subcarpeta del proyecto y compilar desde ahí
WORKDIR /src/SuperPrecioApp
RUN dotnet publish "SuperPrecioApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Imagen final de ejecución
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Crear una carpeta interna para la base de datos SQLite persistente
RUN mkdir -p /app/data

# Punto de entrada apuntando a la DLL compilada de tu proyecto
ENTRYPOINT ["dotnet", "SuperPrecioApp.dll"]