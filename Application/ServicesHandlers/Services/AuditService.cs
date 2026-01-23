namespace Application.ServicesHandlers.Services;

public interface IAuditService
{
    Task LogEventAsync(string eventType, string eventName, string? description = null, Guid? userId = null, string? userEmail = null, string? additionalData = null, CancellationToken cancellationToken = default);
}

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _dbContext;

    public AuditService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LogEventAsync(string eventType, string eventName, string? description = null, Guid? userId = null, string? userEmail = null, string? additionalData = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var auditLog = new Domain.Entities.AuditLogs.AuditLog(
                eventType: eventType,
                eventName: eventName,
                description: description,
                userId: userId,
                userEmail: userEmail,
                additionalData: additionalData
            );

            await _dbContext.AuditLogs.AddAsync(auditLog, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            // Silently fail to prevent audit logging from breaking the main flow
            // In production, consider logging to a separate system or queue
        }
    }
}
