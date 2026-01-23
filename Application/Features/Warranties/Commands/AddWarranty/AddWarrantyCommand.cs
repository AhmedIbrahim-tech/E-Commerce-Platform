using Application.Common.Bases;

namespace Application.Features.Warranties.Commands.AddWarranty;

public record AddWarrantyCommand(
    string Name,
    string? Description,
    int Duration,
    string DurationPeriod,
    bool IsActive = true
) : IRequest<ApiResponse<string>>;
