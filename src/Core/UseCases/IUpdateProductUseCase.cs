using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.UseCases.Products.Commands; // For UpdateProductCommand

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for updating an existing product.
/// </summary>
public interface IUpdateProductUseCase
{
    /// <summary>
    /// Executes the use case to update an existing product.
    /// </summary>
    /// <param name="command">The command containing the updated details of the product.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the update was successful, otherwise false.</returns>
    ValueTask<bool> ExecuteAsync(UpdateProductCommand command, CancellationToken cancellationToken = default);
}
