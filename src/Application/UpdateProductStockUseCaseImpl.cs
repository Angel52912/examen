using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.Core.UseCases.Products.Commands;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para actualizar el stock de un producto.
/// </summary>
public sealed class UpdateProductStockUseCaseImpl(IProductRepository productRepository) : IUpdateProductStockUseCase
{
    public async ValueTask<bool> ExecuteAsync(UpdateProductStockCommand command, CancellationToken cancellationToken = default)
    {
        if (command.ProductID <= 0)
        {
            throw new ArgumentException("El ID del producto debe ser mayor a cero.", nameof(command.ProductID));
        }

        if (command.NewStock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command.NewStock), "El stock no puede ser negativo.");
        }

        return await productRepository.UpdateStockAsync(command.ProductID, command.NewStock, cancellationToken);
    }
}
