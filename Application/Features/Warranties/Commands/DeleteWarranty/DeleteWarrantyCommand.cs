using Application.Common.Bases;

namespace Application.Features.Warranties.Commands.DeleteWarranty;

public record DeleteWarrantyCommand(Guid Id) : IRequest<ApiResponse<string>>;
