using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para buscar ventas con filtros.
/// </summary>
public sealed class FetchSalesByFilterUseCaseImpl(ISaleRepository saleRepository) : IFetchSalesByFilterUseCase
{
    public IAsyncEnumerable<Sale> ExecuteAsync(SaleFilter filter, CancellationToken cancellationToken = default)
    {
        return saleRepository.FindAsync(filter, cancellationToken);
    }
}
