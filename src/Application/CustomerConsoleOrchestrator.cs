using System;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;

namespace UtmMarket.Application;

/// <summary>
/// Orquestador para la interfaz de usuario de clientes (Ejercicio 1).
/// Gestiona el registro y visualización de clientes para estrategias de lealtad.
/// </summary>
public sealed class CustomerConsoleOrchestrator(ICustomerRepository customerRepository)
{
    public async Task RunMenuAsync(CancellationToken ct = default)
    {
        bool exitRequested = false;
        while (!exitRequested)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("   UTM MARKET - GESTIÓN DE CLIENTES     ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Listar todos los clientes");
            Console.WriteLine("2. Registrar nuevo cliente");
            Console.WriteLine("3. Volver al menú principal");
            Console.WriteLine("----------------------------------------");
            Console.Write("Seleccione una opción: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": await ListCustomersAsync(ct); break;
                case "2": await RegisterCustomerAsync(ct); break;
                case "3": exitRequested = true; break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Opción no válida. Presione cualquier tecla...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ListCustomersAsync(CancellationToken ct)
    {
        Console.WriteLine();
        Console.WriteLine("--- LISTADO DE CLIENTES (LEALTAD) ---");
        Console.WriteLine($"{"ID",-5} | {"NOMBRE",-25} | {"EMAIL",-30} | {"REGISTRO",-12}");
        Console.WriteLine(new string('-', 80));

        int count = 0;
        await foreach (var c in customerRepository.GetAllAsync(ct))
        {
            Console.WriteLine($"{c.CustomerID,-5} | {c.Name,-25} | {c.Email,-30} | {c.RegistrationDate:yyyy-MM-dd}");
            count++;
        }

        if (count == 0) Console.WriteLine("No hay clientes registrados.");
        
        Console.WriteLine();
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    private async Task RegisterCustomerAsync(CancellationToken ct)
    {
        Console.WriteLine();
        Console.WriteLine("--- REGISTRO DE CLIENTE ---");
        
        string name = Prompt("Nombre completo: ");
        string email = PromptEmail("Correo electrónico: ");

        try
        {
            var customer = new Customer(0, name, email);
            var result = await customerRepository.AddAsync(customer, ct);
            Console.WriteLine();
            Console.WriteLine($"[ÉXITO] Cliente registrado con ID: {result.CustomerID}");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"[ERROR] No se pudo registrar: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    private string Prompt(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? v = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(v)) return v;
            Console.WriteLine("  [!] Este campo es obligatorio.");
        }
    }

    private string PromptEmail(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
            {
                return email;
            }
            Console.WriteLine("  [!] Formato de email inválido (ejemplo@utm.com).");
        }
    }
}
