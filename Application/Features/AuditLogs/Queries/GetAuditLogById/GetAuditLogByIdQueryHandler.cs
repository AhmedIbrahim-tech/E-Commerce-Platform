using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;

namespace Application.Features.AuditLogs.Queries.GetAuditLogById;

public class GetAuditLogByIdQueryHandler : ApiResponseHandler,
    IRequestHandler<GetAuditLogByIdQuery, ApiResponse<GetAuditLogByIdResponse>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAuditLogByIdQueryHandler(ApplicationDbContext dbContext) : base()
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<GetAuditLogByIdResponse>> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
    {
        var auditLog = await _dbContext.AuditLogs
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
