# .NET 10 Console Project: Native AOT & Clean Architecture Setup

Este documento detalla la configuración inicial de un proyecto de consola en .NET 10, optimizado para Native AOT y diseñado con principios de Clean Code y Zero Trust, siguiendo las directrices de un Senior Software Architect.

## 1. Resumen de Instalación de Dependencias

Se han instalado los siguientes paquetes NuGet, seleccionados por su compatibilidad con .NET 10 y su idoneidad para aplicaciones Native AOT, priorizando el rendimiento y la minimización de la reflexión en tiempo de ejecución.

| Paquete NuGet                       | Versión | Rol Arquitectónico                                                                | Notas de Compatibilidad AOT                                                                                   |
| :---------------------------------- | :------ | :-------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------ |
| `Microsoft.Data.SqlClient`          | 6.1.4   | Driver SQL Server oficial.                                                        | Optimizada para Native AOT, evita gran parte de la reflexión de versiones anteriores.                         |
| `Microsoft.Extensions.Hosting`      | 10.0.3  | Gestión del ciclo de vida de la aplicación, inyección de dependencias (DI).         | Esencial para un diseño modular y desacoplado. Compatible con AOT mediante `HostApplicationBuilder`.            |
| `Microsoft.Extensions.Configuration.UserSecrets` | 10.0.3  | Configuración segura para desarrollo local (secretos de usuario).               | Facilita la gestión de credenciales sin hardcodearlas. Compatible con AOT.                                    |
| `Dapper`                            | (Omitido)| Micro-ORM.                                                                        | **Omitido:** Su dependencia en reflexión en tiempo de ejecución lo hace incompatible con Native AOT estricto. Se priorizará ADO.NET puro o alternativas Source-Generated. |

## 2. Referencia de Implementación: `Program.cs` Esqueleto Funcional

El siguiente código proporciona un esqueleto de `Program.cs` que demuestra una configuración básica utilizando `HostApplicationBuilder` para DI, un servicio de ejemplo con operaciones asíncronas y manejo de `CancellationToken`, todo ello manteniendo la compatibilidad con Native AOT.

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient; // Assuming direct ADO.NET usage for AOT

// Define a simple service for demonstration, avoiding reflection
public interface IMyService
{
    ValueTask<string> GetGreetingAsync(CancellationToken cancellationToken = default);
}

public class MyService : IMyService
{
    // C# 14 'field' keyword example (for properties, not directly in Program.cs top-level)
    // public string MyProperty { get => field; set => field = value; }
    // The 'field' keyword is syntactic sugar for backing fields in properties,
    // enhancing readability and reducing boilerplate. Its usage is demonstrated
    // conceptually here as it's typically used within class property definitions,
    // not top-level statements.

    public async ValueTask<string> GetGreetingAsync(CancellationToken cancellationToken = default)
    {
        // Simulate an async operation without reflection-heavy ORMs
        await Task.Delay(100, cancellationToken);
        return "Hello from .NET 10 AOT-optimized Console App!";
    }
}

// HostApplicationBuilder for simplified and AOT-friendly setup
var builder = Host.CreateApplicationBuilder(args);

// Configure services for Dependency Injection
builder.Services.AddSingleton<IMyService, MyService>();
builder.Services.AddTransient<SqlConnection>(); // Example: Registering SQL connection

// Add User Secrets configuration for development
// builder.Configuration.AddUserSecrets<Program>(); // Uncomment and configure UserSecrets ID in .csproj if needed

// Build the host
using var host = builder.Build();

// Get the service and execute business logic
var myService = host.Services.GetRequiredService<IMyService>();

// Use CancellationToken for cancellable operations
using var cts = new CancellationTokenSource();
try
{
    var greeting = await myService.GetGreetingAsync(cts.Token);
    Console.WriteLine(greeting);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

// Example of accessing configuration (e.g., from appsettings.json or User Secrets)
// var configValue = builder.Configuration["MySetting"];
// Console.WriteLine($"Configured setting: {configValue ?? "Not found"}");

await host.RunAsync();

// For Native AOT, ensure all types used by reflection (if any) are configured for source generation
// For example, if using System.Text.Json, use JsonSerializerContext
// [JsonSerializable(typeof(MyDataModel))]
// internal partial class AppJsonSerializerContext : JsonSerializerContext { }
```

## 3. Notas de Modernización en .NET 10 (C# 14)

### Beneficios del uso de `field` (C# 14)
La palabra clave `field` en C# 14 simplifica la creación de propiedades con backing fields. Permite acceder directamente al campo de respaldo de una propiedad autoimplementada desde sus accesores `get` y `set`. Esto reduce el código boilerplate, mejorando la legibilidad y el mantenimiento, especialmente en escenarios donde se necesita lógica adicional en los accesores sin tener que declarar explícitamente el campo de respaldo. Aunque no se demostró directamente en el `Program.cs` de ejemplo (por su naturaleza de top-level statements), es una característica potente para la implementación de clases de modelo y servicios.

### Beneficios de Native AOT
Native Ahead-of-Time (AOT) compilation en .NET 10 ofrece mejoras significativas para aplicaciones de consola:
*   **Inicio Más Rápido:** Al compilar el código directamente a código máquina nativo, se elimina la necesidad de compilación JIT (Just-In-Time) en tiempo de ejecución, resultando en tiempos de inicio mucho más rápidos, crucial para herramientas CLI y microservicios serverless.
*   **Menor Consumo de Memoria:** Los binarios nativos suelen tener una huella de memoria más pequeña debido a la eliminación de componentes del runtime JIT y a optimizaciones específicas para el árbol de llamadas conocido en tiempo de compilación.
*   **Binarios de un Solo Archivo:** Facilita la distribución al empaquetar toda la aplicación y sus dependencias en un único archivo ejecutable.
*   **Seguridad Mejorada:** La reducción de la superficie de ataque al no incluir el compilador JIT y otras herramientas de desarrollo en el binario final.
*   **"Physical Promotion" y Desvirtualización de Interfaces:** Las optimizaciones del runtime de .NET 10 aprovechan mejor la información disponible en tiempo de compilación para desvirtualizar llamadas a métodos e interfaces, lo que resulta en un código más eficiente y un mejor rendimiento general.

## 4. Guía de Ejecución: Compilación como Binario Nativo

Para compilar este proyecto como un binario nativo que aproveche las optimizaciones de .NET 10 AOT, sigue estos pasos:

1.  **Asegúrate de tener el SDK de .NET 10 instalado.**
2.  **Navega a la carpeta raíz del proyecto** (`C:\Users\angel\utmmarket`).
3.  **Ejecuta el siguiente comando:**

    ```bash
    dotnet publish -r <RID> -c Release /p:PublishAot=true --self-contained
    ```
    Donde `<RID>` es el Runtime Identifier (identificador de tiempo de ejecución) para tu plataforma objetivo. Algunos ejemplos comunes:
    *   `win-x64` (Windows de 64 bits)
    *   `linux-x64` (Linux de 64 bits)
    *   `osx-x64` (macOS de 64 bits)

    **Ejemplo para Windows de 64 bits:**
    ```bash
    dotnet publish -r win-x64 -c Release /p:PublishAot=true --self-contained
    ```

    Este comando generará un ejecutable nativo optimizado en la carpeta `bin\Release
et10.0\<RID>\publish`. Este binario no tendrá dependencias externas del runtime de .NET y estará listo para su despliegue en la plataforma especificada.
