## Flujo de trabajo con Git

### 1. Clonar el repositorio

Descarga el repositorio a tu ordenador:

```bash
git clone https://github.com/MiguelAngelGiraldoPolanco/SuperPrecioApp.git
cd SuperPrecioApp
```

### 2. Descargar los últimos cambios

Antes de empezar a trabajar, actualiza la información del repositorio remoto:

```bash
git switch main
git pull origin main
```

### 3. Crear una rama de trabajo

Cada desarrollador debe trabajar en su propia rama. Utiliza un nombre descriptivo para la tarea:

```bash
git switch -c NombreDeCadaUno
```

Ejemplos de nombres de ramas:

- `Daniel`
- `Pablo`
- `Miguel`
- `JoseLuis`

### 4. Programar y guardar los cambios

Comprueba qué archivos has modificado:

```bash
git status
```

Añade los archivos que quieras incluir en el commit:

```bash
git add .
```

Crea un commit con un mensaje descriptivo:

```bash
git commit -m "Crear modelo usuario"
```

### 5. Subir la rama a GitHub

Publica tu rama en el repositorio remoto:

```bash
git push -u origin NombreDeLaRamaEnLaQueTrabajas
```

Si la rama ya está publicada, los siguientes cambios se suben con:

```bash
git push
```

### 6. Crear un Pull Request

1. Entra en el repositorio de GitHub.
2. Abre la sección **Pull requests**.
3. Pulsa **New pull request**.
4. Selecciona `main` como rama de destino y tu rama como rama de origen.
5. Describe los cambios realizados y crea el Pull Request.
6. Espera a que se revise el código antes de fusionarlo con `main`.

### 7. Actualizar tu rama antes de continuar

Si se han incorporado cambios a `main` mientras trabajabas:

```bash
git switch main
git pull origin main
git switch NombreDeLaRamaDeCadaUno
git merge main
```

Resuelve los conflictos, si aparecen, y continúa trabajando.

### Normas de colaboración

- No trabajar directamente sobre `main`.
- Crear una rama por compañero.
- Realizar commits pequeños y descriptivos.
- No subir contraseñas, tokens, claves de API ni bases de datos con información sensible.
- Crear un Pull Request para integrar los cambios en `main`.
- Revisar y probar el código antes de fusionarlo.

### Arquitectura

```text
SuperPrecioApp/
├── superPrecioApp.cs
├── Models/
│   ├── Producto.cs
│   └── Usuario.cs
├── Views/
│   └── MenuView.cs
├── Controllers/
│   └── ProductoController.cs
├── Services/
│   └── ApiExternaService.cs
├── Data/
│   ├── DatabaseConnection.cs
│   └── ProductoRepository.cs
├── appsettings.json
├── .gitignore
└── README.md

###Explicacion de la Arquitectura 

Models/: clases que representan los datos que maneja la aplicación.

Views/: menús, entrada del usuario y presentación de resultados por consola.

Controllers/: coordinan las acciones del usuario y las operaciones.

Services/: contiene ApiExternaService, que realiza las peticiones HTTP a la API de terceros.

Data/: gestiona SQLite, las conexiones y las consultas.

DTOs/: representa los datos recibidos de la API externa cuando su estructura difiere de vuestros modelos.
