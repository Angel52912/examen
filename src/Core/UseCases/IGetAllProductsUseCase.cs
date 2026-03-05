using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for retrieving all products.
/// </summary>
public interface IGetAllProductsUseCase
{
    /// <summary>
    /// Executes the use case to retrieve all products.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of products.</returns>
    IAsyncEnumerable<Product> ExecuteAsync(CancellationToken cancellationToken = default);
}
