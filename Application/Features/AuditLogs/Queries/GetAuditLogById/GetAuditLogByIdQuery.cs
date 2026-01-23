using Application.Common.Bases;

namespace Application.Features.AuditLogs.Queries.GetAuditLogById;

public record GetAuditLogByIdQuery(Guid Id) : IRequest<ApiResponse<GetAuditLogByIdResponse>>;
