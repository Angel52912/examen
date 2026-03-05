using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for retrieving all sales.
/// </summary>
public interface IFetchAllSalesUseCase
{
    /// <summary>
    /// Executes the use case to retrieve all sales.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of sales.</returns>
    IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken cancellationToken = default);
}
