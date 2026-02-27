using Domain.Entities.AuditLogs;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.ServicesHandlers.Services;

public interface IAuditService
{
    Task LogEventAsync(string eventType, string eventName, string? description = null, Guid? userId = null, string? userEmail = null, string? additionalData = null, CancellationToken cancellationToken = default);
}

public class AuditService : IAuditService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task LogEventAsync(string eventType, string eventName, string? description = null, Guid? userId = null, string? userEmail = null, string? additionalData = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var auditLog = new AuditLog(
                eventType: eventType,
                eventName: eventName,
                description: description,
                userId: userId,
                userEmail: userEmail,
                additionalData: additionalData
            );

            await _unitOfWork.AuditLogs.AddAsync(auditLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
        }
    }
}
