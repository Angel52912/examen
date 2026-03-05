using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para recuperar todas las ventas.
/// </summary>
public sealed class FetchAllSalesUseCaseImpl(ISaleRepository saleRepository) : IFetchAllSalesUseCase
{
    public IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return saleRepository.GetAllAsync(cancellationToken);
    }
}
