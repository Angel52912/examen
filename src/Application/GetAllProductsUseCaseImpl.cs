using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para recuperar todos los productos.
/// </summary>
public sealed class GetAllProductsUseCaseImpl(IProductRepository productRepository) : IGetAllProductsUseCase
{
    public IAsyncEnumerable<Product> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return productRepository.GetAllAsync(cancellationToken);
    }
}
