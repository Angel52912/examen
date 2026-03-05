using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for retrieving a product by its unique identifier.
/// </summary>
public interface IGetProductByIdUseCase
{
    /// <summary>
    /// Executes the use case to retrieve a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product, or null if not found.</returns>
    ValueTask<Product?> ExecuteAsync(int productId, CancellationToken cancellationToken = default);
}
