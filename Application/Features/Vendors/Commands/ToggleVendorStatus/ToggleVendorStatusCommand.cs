using Application.Common.Bases;
using MediatR;

namespace Application.Features.Vendors.Commands.ToggleVendorStatus;

public record ToggleVendorStatusCommand(Guid Id) : IRequest<ApiResponse<string>>;
