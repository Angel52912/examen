using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para buscar productos con filtros.
/// </summary>
public sealed class FindProductsUseCaseImpl(IProductRepository productRepository) : IFindProductsUseCase
{
    public IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken cancellationToken = default)
    {
        return productRepository.FindAsync(filter, cancellationToken);
    }
}
