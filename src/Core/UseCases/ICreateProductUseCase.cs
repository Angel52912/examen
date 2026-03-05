using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.UseCases.Products.Commands; // For CreateProductCommand

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for creating a new product.
/// </summary>
public interface ICreateProductUseCase
{
    /// <summary>
    /// Executes the use case to create a new product.
    /// </summary>
    /// <param name="command">The command containing the details of the product to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created product with its generated identity.</returns>
    ValueTask<Product> ExecuteAsync(CreateProductCommand command, CancellationToken cancellationToken = default);
}
