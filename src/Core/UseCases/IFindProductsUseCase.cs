using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters; // For ProductFilter

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for searching products based on specified filtering criteria.
/// </summary>
public interface IFindProductsUseCase
{
    /// <summary>
    /// Executes the use case to search for products based on specified filtering criteria.
    /// </summary>
    /// <param name="filter">The criteria to apply for filtering products.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of products that match the filter criteria.</returns>
    IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken cancellationToken = default);
}
