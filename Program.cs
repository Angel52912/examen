using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UtmMarket.Infrastructure; // For AddInfrastructureServices
using UtmMarket.Application; // For AddApplicationServices
using System; // For Console, Exception
using System.Threading.Tasks; // For Task

var builder = Host.CreateApplicationBuilder(args);

// Explicitly add configuration sources
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

if (builder.Environment.IsDevelopment())
{
    // Cargar secretos de usuario si el entorno es Desarrollo
    builder.Configuration.AddUserSecrets<Program>();
}

// Registro de servicios de las capas inferiores
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var host = builder.Build();

// Arte ASCII profesional y mensaje de inicio
Console.Clear();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("""
    ********************************************************
    *                                                      *
    *      __  __________  ___  ___  ___  ___  _____ ___   *
    *     / / / /_  __/  |/  / / _ \/ _ \/ _ \/ _ \/  _/   *
    *    / /_/ / / / / /|_/ / / ___/ , _/ // / ___// /     *
    *    \____/ /_/ /_/  /_/ /_/  /_/|_/____/_/  /___/     *
    *                                                      *
    *               SISTEMA DE GESTIÓN UTM                 *
    *                                                      *
    ********************************************************
    """);
Console.ResetColor();
Console.WriteLine("\nIniciando sistema central...");
await Task.Delay(1000);

bool mainExit = false;
while (!mainExit)
{
    Console.Clear();
    Console.WriteLine("""
        ========================================
             SISTEMA CENTRAL - UTM MARKET
        ========================================
        1. Módulo de Productos
        2. Reporte de Ventas (Historial)
        3. Gestión de Clientes (Lealtad)
        4. Salir
        ----------------------------------------
        """);
    Console.Write("Seleccione Módulo: ");
    string? moduleChoice = Console.ReadLine();

    // RESTRICCIÓN TÉCNICA: IServiceScope manual por ciclo
    using (var scope = host.Services.CreateScope())
    {
        var sp = scope.ServiceProvider;

        try
        {
            switch (moduleChoice)
            {
                case "1":
                    var prodOrchestrator = sp.GetRequiredService<ProductConsoleOrchestrator>();
                    await prodOrchestrator.RunMenuAsync();
                    break;
                case "2":
                    var saleOrchestrator = sp.GetRequiredService<SaleConsoleOrchestrator>();
                    await saleOrchestrator.RunHistoryQueryAsync();
                    break;
                case "3":
                    var customerOrchestrator = sp.GetRequiredService<CustomerConsoleOrchestrator>();
                    await customerOrchestrator.RunMenuAsync();
                    break;
                case "4":
                    mainExit = true;
                    break;
                default:
                    Console.WriteLine("\nOpción inválida. Presione cualquier tecla...");
                    Console.ReadKey();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERROR] Se produjo un fallo en el módulo: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para continuar.");
            Console.ReadKey();
        }
    }
}

Console.WriteLine("\nGracias por utilizar UTM Market. ¡Hasta pronto!");
