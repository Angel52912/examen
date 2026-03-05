using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.Core.UseCases.Products.Commands;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para crear un producto.
/// </summary>
/// <param name="productRepository">El repositorio de productos inyectado.</param>
public sealed class CreateProductUseCaseImpl(IProductRepository productRepository) : ICreateProductUseCase
{
    public async ValueTask<Product> ExecuteAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        // Validación básica
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(command));
        }

        if (string.IsNullOrWhiteSpace(command.SKU))
        {
            throw new ArgumentException("El SKU del producto no puede estar vacío.", nameof(command));
        }

        if (command.Price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command), "El precio no puede ser negativo.");
        }

        if (command.Stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command), "El stock no puede ser negativo.");
        }

        var product = new Product(
            productId: 0, // El ID será generado por la base de datos
            name: command.Name,
            sku: command.SKU,
            brand: command.Brand,
            price: command.Price,
            stock: command.Stock
        );

        return await productRepository.AddAsync(product, cancellationToken);
    }
}
