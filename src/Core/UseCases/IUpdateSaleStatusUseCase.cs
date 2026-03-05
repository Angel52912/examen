using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.UseCases.Sales.Commands; // For UpdateSaleStatusCommand

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for updating the status of an existing sale.
/// </summary>
public interface IUpdateSaleStatusUseCase
{
    /// <summary>
    /// Executes the use case to update the status of a specific sale.
    /// </summary>
    /// <param name="command">The command containing the sale ID and the new status.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the status update was successful, otherwise false.</returns>
    ValueTask<bool> ExecuteAsync(UpdateSaleStatusCommand command, CancellationToken cancellationToken = default);
}
