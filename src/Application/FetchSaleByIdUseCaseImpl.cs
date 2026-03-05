using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para buscar una venta por ID.
/// </summary>
public sealed class FetchSaleByIdUseCaseImpl(ISaleRepository saleRepository) : IFetchSaleByIdUseCase
{
    public ValueTask<Sale?> ExecuteAsync(int saleId, CancellationToken cancellationToken = default)
    {
        return new ValueTask<Sale?>(saleRepository.GetByIdAsync(saleId, cancellationToken));
    }
}
