namespace Application.Features.Warranties.Queries.GetWarrantyList;

public record GetWarrantyListResponse(
    Guid Id,
    string Name,
    string? Description,
    int Duration,
    string DurationPeriod,
    bool IsActive
);
