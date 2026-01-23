using Application.Common.Bases;

namespace Application.Features.Warranties.Queries.GetWarrantyById;

public record GetWarrantyByIdQuery(Guid Id) : IRequest<ApiResponse<GetWarrantyByIdResponse>>;
