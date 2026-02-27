using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.RepositoriesHandlers.Bases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoriesHandlers.Repositories.Users;

public interface ICustomerRepository : IGenericRepositoryAsync<Customer>
{
    Task<Customer?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
}

public class CustomerRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Customer>(dbContext), ICustomerRepository
{
    public async Task<Customer?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .FirstOrDefaultAsync(c => c.AppUserId == appUserId, cancellationToken);
    }

    public async Task<bool> ExistsByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .AnyAsync(c => c.AppUserId == appUserId, cancellationToken);
    }
}
