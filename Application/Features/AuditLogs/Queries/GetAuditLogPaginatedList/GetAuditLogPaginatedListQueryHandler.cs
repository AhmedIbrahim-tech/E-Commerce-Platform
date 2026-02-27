using Application.Common.Bases;
using Application.Wrappers;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.AuditLogs.Queries.GetAuditLogPaginatedList;

public class GetAuditLogPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetAuditLogPaginatedListQuery, PaginatedResult<GetAuditLogPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetAuditLogPaginatedListResponse>> Handle(GetAuditLogPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var auditLogsQuery = unitOfWork.AuditLogs.GetTableNoTracking()
            .ApplyFiltering(
                request.SortBy,
                request.Search,
                request.UserId,
                request.EventType,
                request.StartDate,
                request.EndDate);

        var auditLogs = await auditLogsQuery
            .Select(log => new GetAuditLogPaginatedListResponse
            {
                Id = log.Id,
                EventType = log.EventType,
                EventName = log.EventName,
                Description = log.Description,
                UserId = log.UserId,
                UserEmail = log.UserEmail,
                AdditionalData = log.AdditionalData,
                CreatedTime = log.CreatedTime
            })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        auditLogs.Meta = new { Count = auditLogs.Data.Count() };
        return auditLogs;
    }
}
