using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.Core.UseCases.Products.Commands;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para actualizar un producto.
/// </summary>
public sealed class UpdateProductUseCaseImpl(IProductRepository productRepository) : IUpdateProductUseCase
{
    public async ValueTask<bool> ExecuteAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        // Validación básica
        if (command.ProductID <= 0)
        {
            throw new ArgumentException("El ID del producto debe ser mayor a cero.", nameof(command.ProductID));
        }

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(command.Name));
        }

        if (string.IsNullOrWhiteSpace(command.SKU))
        {
            throw new ArgumentException("El SKU del producto no puede estar vacío.", nameof(command.SKU));
        }

        if (command.Price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command.Price), "El precio no puede ser negativo.");
        }

        if (command.Stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command.Stock), "El stock no puede ser negativo.");
        }

        var product = new Product(
            productId: command.ProductID,
            name: command.Name,
            sku: command.SKU,
            brand: command.Brand,
            price: command.Price,
            stock: command.Stock
        );

        return await productRepository.UpdateAsync(product, cancellationToken);
    }
}
