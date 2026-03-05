using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;

namespace UtmMarket.Core.Repositories;

public interface ICustomerRepository
{
    IAsyncEnumerable<Customer> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
}
