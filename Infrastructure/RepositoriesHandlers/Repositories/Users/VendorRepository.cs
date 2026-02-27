using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.RepositoriesHandlers.Bases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoriesHandlers.Repositories.Users;

public interface IVendorRepository : IGenericRepositoryAsync<Vendor>
{
    Task<Vendor?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
}

public class VendorRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Vendor>(dbContext), IVendorRepository
{
    public async Task<Vendor?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .FirstOrDefaultAsync(v => v.AppUserId == appUserId, cancellationToken);
    }

    public async Task<bool> ExistsByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .AnyAsync(v => v.AppUserId == appUserId, cancellationToken);
    }
}
