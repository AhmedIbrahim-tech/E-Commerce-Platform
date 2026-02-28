using Domain.Entities.AuditLogs;

namespace Application.ServicesHandlers.Services;

public interface IAuditService
{
    Task LogEventAsync(string eventType, string eventName, string? description = null, object? userId = null, string? userEmail = null, string? additionalData = null, CancellationToken cancellationToken = default);
}

public class AuditService : IAuditService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task LogEventAsync(string eventType, string eventName, string? description = null, object? userId = null, string? userEmail = null, string? additionalData = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var resolvedUserId = userId is Guid id ? id : (Guid?)null;
            var auditLog = new AuditLog(
                eventType,
                eventName,
                description,
                resolvedUserId,
                userEmail,
                additionalData);

            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
        }
    }
}
