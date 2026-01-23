using Application.Common.Bases;

namespace Application.Features.Warranties.Commands.EditWarranty;

public record EditWarrantyCommand(
    Guid Id,
    string Name,
    string? Description,
    int Duration,
    string DurationPeriod,
    bool IsActive
) : IRequest<ApiResponse<string>>;
