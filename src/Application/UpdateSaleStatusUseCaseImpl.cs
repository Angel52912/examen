using System;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.Core.UseCases.Sales.Commands;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para actualizar el estado de una venta.
/// </summary>
public sealed class UpdateSaleStatusUseCaseImpl(ISaleRepository saleRepository) : IUpdateSaleStatusUseCase
{
    public async ValueTask<bool> ExecuteAsync(UpdateSaleStatusCommand command, CancellationToken cancellationToken = default)
    {
        if (command.SaleID <= 0)
        {
            throw new ArgumentException("El ID de la venta debe ser mayor a cero.", nameof(command.SaleID));
        }

        var sale = await saleRepository.GetByIdAsync(command.SaleID, cancellationToken);
        if (sale == null)
        {
            return false;
        }

        sale.Status = command.NewStatus;
        await saleRepository.UpdateAsync(sale, cancellationToken);
        return true;
    }
}
