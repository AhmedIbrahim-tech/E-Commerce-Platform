using Domain.Entities.AuditLogs;
using Infrastructure.Data;
using Infrastructure.RepositoriesHandlers.Bases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IAuditLogRepository : IGenericRepositoryAsync<AuditLog>
{
    Task<IReadOnlyList<AuditLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditLog>> GetByEventTypeAsync(string eventType, CancellationToken cancellationToken = default);
}

public class AuditLogRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<AuditLog>(dbContext), IAuditLogRepository
{
    public async Task<IReadOnlyList<AuditLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>> GetByEventTypeAsync(string eventType, CancellationToken cancellationToken = default)
    {
        return await GetTableNoTracking()
            .Where(a => a.EventType == eventType)
            .OrderByDescending(a => a.CreatedTime)
            .ToListAsync(cancellationToken);
    }
}
