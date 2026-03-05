using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters;

namespace UtmMarket.Core.Repositories;

/// <summary>
/// Defines the contract for a sales repository, managing persistence operations for the Sale aggregate root.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Retrieves all sales in an asynchronous stream.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of sales.</returns>
    IAsyncEnumerable<Sale> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single sale by its unique identifier.
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the sale, or null if not found.</returns>
    Task<Sale?> GetByIdAsync(int saleId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for sales based on specified filtering criteria.
    /// </summary>
    /// <param name="filter">The criteria to apply for filtering sales.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of sales that match the filter criteria.</returns>
    IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new sale to the repository.
    /// </summary>
    /// <param name="sale">The sale aggregate root to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the persisted sale with its generated identity.</returns>
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale in the repository.
    /// </summary>
    /// <param name="sale">The sale aggregate root to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
}
