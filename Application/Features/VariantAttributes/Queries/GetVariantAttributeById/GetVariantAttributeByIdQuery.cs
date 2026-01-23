using Application.Common.Bases;

namespace Application.Features.VariantAttributes.Queries.GetVariantAttributeById;

public record GetVariantAttributeByIdQuery(Guid Id) : IRequest<ApiResponse<GetVariantAttributeByIdResponse>>;
