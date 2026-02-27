using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.AuditLogs.Queries.GetAuditLogById;

public class GetAuditLogByIdQueryHandler(
    IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetAuditLogByIdQuery, ApiResponse<GetAuditLogByIdResponse>>
{
    public async Task<ApiResponse<GetAuditLogByIdResponse>> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
    {
        var auditLog = await unitOfWork.AuditLogs.GetTableNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (auditLog is null) 
            return new ApiResponse<GetAuditLogByIdResponse>(AuditLogErrors.AuditLogNotFound());

        var auditLogResponse = new GetAuditLogByIdResponse
        {
            Id = auditLog.Id,
            EventType = auditLog.EventType,
            EventName = auditLog.EventName,
            Description = auditLog.Description,
            UserId = auditLog.UserId,
            UserEmail = auditLog.UserEmail,
            AdditionalData = auditLog.AdditionalData,
            CreatedTime = auditLog.CreatedTime
        };

        return Success(auditLogResponse);
    }
}
