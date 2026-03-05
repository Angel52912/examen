using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for retrieving a sale by its unique identifier.
/// </summary>
public interface IFetchSaleByIdUseCase
{
    /// <summary>
    /// Executes the use case to retrieve a sale by its unique identifier.
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the sale, or null if not found.</returns>
    ValueTask<Sale?> ExecuteAsync(int saleId, CancellationToken cancellationToken = default);
}
