using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Filters;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Orquestador para la interfaz de usuario de ventas.
/// Gestiona la captura de rangos de fechas y la presentación de resultados.
/// </summary>
public sealed class SaleConsoleOrchestrator(IFetchSalesByFilterUseCase fetchSalesUseCase)
{
    public async Task RunHistoryQueryAsync(CancellationToken ct = default)
    {
        Console.Clear();
        Console.WriteLine("====================================================");
        Console.WriteLine("   UTM MARKET - CONSULTA DE HISTORIAL DE VENTAS     ");
        Console.WriteLine("====================================================");
        Console.WriteLine();

        DateTime startDate = PromptDate("Ingrese Fecha de Inicio (yyyy-mm-dd): ");
        DateTime endDate = PromptDate("Ingrese Fecha de Fin    (yyyy-mm-dd): ");

        if (startDate > endDate)
        {
            Console.WriteLine();
            Console.WriteLine("[ERROR] La fecha de inicio no puede ser mayor a la fecha de fin.");
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
            return;
        }

        var filter = new SaleFilter 
        { 
            StartDate = startDate, 
            EndDate = endDate 
        };

        Console.WriteLine();
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"{"FOLIO",-15} | {"FECHA",-12} | {"TOTAL",15}");
        Console.WriteLine("----------------------------------------------------");

        decimal totalPeriodo = 0;
        int count = 0;

        await foreach (var sale in fetchSalesUseCase.ExecuteAsync(filter, ct))
        {
            Console.WriteLine($"{sale.Folio,-15} | {sale.SaleDate:yyyy-MM-dd} | {sale.TotalSale,15:C}");
            totalPeriodo += sale.TotalSale;
            count++;
        }

        Console.WriteLine("----------------------------------------------------");
        if (count > 0)
        {
            Console.WriteLine($"TOTAL DEL PERIODO ({count} ventas): {totalPeriodo,24:C}");
        }
        else
        {
            Console.WriteLine("No se encontraron ventas en el rango seleccionado.");
        }

        Console.WriteLine();
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    private DateTime PromptDate(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            if (DateTime.TryParse(input, out DateTime dateAlt))
            {
                return dateAlt;
            }
            Console.WriteLine("  [!] Formato inválido. Use AAAA-MM-DD. Intente de nuevo.");
        }
    }
}
