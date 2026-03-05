using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters; // For SaleFilter

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for searching sales based on specified filtering criteria.
/// </summary>
public interface IFetchSalesByFilterUseCase
{
    /// <summary>
    /// Executes the use case to search for sales based on specified filtering criteria.
    /// </summary>
    /// <param name="filter">The criteria to apply for filtering sales.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of sales that match the filter criteria.</returns>
    IAsyncEnumerable<Sale> ExecuteAsync(SaleFilter filter, CancellationToken cancellationToken = default);
}
