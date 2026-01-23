using Application.Common.Bases;

namespace Application.Features.Warranties.Queries.GetWarrantyList;

public record GetWarrantyListQuery() : IRequest<ApiResponse<List<GetWarrantyListResponse>>>;
