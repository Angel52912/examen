using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para buscar un producto por ID.
/// </summary>
public sealed class GetProductByIdUseCaseImpl(IProductRepository productRepository) : IGetProductByIdUseCase
{
    public ValueTask<Product?> ExecuteAsync(int productId, CancellationToken cancellationToken = default)
    {
        return new ValueTask<Product?>(productRepository.GetByIdAsync(productId, cancellationToken));
    }
}
