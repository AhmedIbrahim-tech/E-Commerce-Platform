using Application.Common.Bases;

namespace Application.Features.Vendors.Queries.GetVendorById;

public record GetVendorByIdQuery(Guid Id) : IRequest<ApiResponse<GetVendorByIdResponse>>;
