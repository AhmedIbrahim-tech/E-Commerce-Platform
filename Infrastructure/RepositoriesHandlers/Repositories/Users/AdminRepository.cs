using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.RepositoriesHandlers.Bases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoriesHandlers.Repositories.Users;

public interface IAdminRepository : IGenericRepositoryAsync<Admin>
{
    Task<Admin?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
}

public class AdminRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Admin>(dbContext), IAdminRepository
{
    public async Task<Admin?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .FirstOrDefaultAsync(a => a.AppUserId == appUserId, cancellationToken);
    }

    public async Task<bool> ExistsByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .AnyAsync(a => a.AppUserId == appUserId, cancellationToken);
    }
}
