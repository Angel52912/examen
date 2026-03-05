using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.UseCases.Products.Commands; // For UpdateProductStockCommand

namespace UtmMarket.Core.UseCases;

/// <summary>
/// Defines the contract for updating the stock of a product.
/// </summary>
public interface IUpdateProductStockUseCase
{
    /// <summary>
    /// Executes the use case to update the stock quantity of a specific product.
    /// </summary>
    /// <param name="command">The command containing the product ID and the new stock quantity.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the stock update was successful, otherwise false.</returns>
    ValueTask<bool> ExecuteAsync(UpdateProductStockCommand command, CancellationToken cancellationToken = default);
}
