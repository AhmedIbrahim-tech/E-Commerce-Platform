using Application.Common.Bases;

namespace Application.Features.Vendors.Commands.DeleteVendor;

public record DeleteVendorCommand(Guid Id) : IRequest<ApiResponse<string>>;
