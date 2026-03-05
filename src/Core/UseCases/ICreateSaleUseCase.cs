using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.UseCases.Sales.Commands; // For CreateSaleCommand

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for creating a new sale.
/// </summary>
public interface ICreateSaleUseCase
{
    /// <summary>
    /// Executes the use case to create a new sale.
    /// </summary>
    /// <param name="command">The command containing the details of the sale to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created sale with its generated identity.</returns>
    ValueTask<Sale> ExecuteAsync(CreateSaleCommand command, CancellationToken cancellationToken = default);
}
