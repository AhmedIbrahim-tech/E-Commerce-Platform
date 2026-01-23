using Application.Common.Bases;

namespace Application.Features.VariantAttributes.Queries.GetVariantAttributeList;

public record GetVariantAttributeListQuery() : IRequest<ApiResponse<List<GetVariantAttributeListResponse>>>;
