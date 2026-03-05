using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.UseCases;
using UtmMarket.Core.UseCases.Products.Commands;

namespace UtmMarket.Application;

/// <summary>
/// Orquestador para la interacción por consola relacionada con la gestión de productos.
/// Centraliza la lógica de captura de datos y validación de entrada.
/// </summary>
public sealed class ProductConsoleOrchestrator(
    IGetAllProductsUseCase getAllProductsUseCase,
    IGetProductByIdUseCase getProductByIdUseCase,
    ICreateProductUseCase createProductUseCase)
{
    public async Task RunMenuAsync(CancellationToken cancellationToken = default)
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.Clear();
            Console.WriteLine("""
                ========================================
                   UTM MARKET - GESTIÓN DE PRODUCTOS
                ========================================
                1. Listar todos los productos
                2. Buscar producto por ID
                3. Registrar nuevo producto
                4. Salir
                ----------------------------------------
                """);
            Console.Write("Seleccione una opción: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ListProductsAsync(cancellationToken);
                    break;
                case "2":
                    await FindProductByIdAsync(cancellationToken);
                    break;
                case "3":
                    await RegisterProductAsync(cancellationToken);
                    break;
                case "4":
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ListProductsAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.WriteLine("--- LISTADO DE PRODUCTOS ---");
        int count = 0;
        await foreach (var product in getAllProductsUseCase.ExecuteAsync(cancellationToken))
        {
            Console.WriteLine($"ID: {product.ProductID,-5} | {product.Name,-20} | SKU: {product.SKU,-10} | Precio: {product.Price,10:C} | Stock: {product.Stock,5}");
            count++;
        }

        if (count == 0)
        {
            Console.WriteLine("No hay productos registrados.");
        }

        Console.WriteLine();
        Console.WriteLine("Presione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }

    private async Task FindProductByIdAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.Write("Ingrese el ID del producto: ");
        if (!int.TryParse(Console.ReadLine(), out int productId))
        {
            Console.WriteLine("Error: El ID debe ser un número entero.");
        }
        else
        {
            var product = await getProductByIdUseCase.ExecuteAsync(productId, cancellationToken);
            if (product == null)
            {
                Console.WriteLine($"No se encontró ningún producto con ID {productId}.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Detalles del Producto:");
                Console.WriteLine($"  ID:      {product.ProductID}");
                Console.WriteLine($"  Nombre:  {product.Name}");
                Console.WriteLine($"  SKU:     {product.SKU}");
                Console.WriteLine($"  Marca:   {product.Brand ?? "N/A"}");
                Console.WriteLine($"  Precio:  {product.Price:C}");
                Console.WriteLine($"  Stock:   {product.Stock}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Presione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }

    private async Task RegisterProductAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.WriteLine("--- REGISTRO DE NUEVO PRODUCTO ---");

        string name = PromptNotEmpty("Nombre: ");
        string sku = PromptNotEmpty("SKU: ");
        Console.Write("Marca (opcional): ");
        string? brand = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(brand)) brand = null;

        decimal price = PromptDecimal("Precio: ");
        int stock = PromptInt("Stock inicial: ");

        try
        {
            var command = new CreateProductCommand(name, sku, brand, price, stock);
            var result = await createProductUseCase.ExecuteAsync(command, cancellationToken);
            Console.WriteLine();
            Console.WriteLine($"Producto '{result.Name}' registrado exitosamente con ID {result.ProductID}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"Error al registrar el producto: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Presione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }

    private string PromptNotEmpty(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? value = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(value)) return value;
            Console.WriteLine("  Error: Este campo es obligatorio.");
        }
    }

    private decimal PromptDecimal(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal value) && value >= 0)
            {
                return value;
            }
            Console.WriteLine("  Error: Ingrese un precio válido (mayor o igual a 0).");
        }
    }

    private int PromptInt(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= 0)
            {
                return value;
            }
            Console.WriteLine("  Error: Ingrese un número entero válido (mayor o igual a 0).");
        }
    }
}
