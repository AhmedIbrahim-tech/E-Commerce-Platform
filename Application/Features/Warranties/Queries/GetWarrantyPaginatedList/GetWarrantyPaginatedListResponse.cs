namespace Application.Features.Warranties.Queries.GetWarrantyPaginatedList;

public record GetWarrantyPaginatedListResponse(
    Guid Id,
    string Name,
    string? Description,
    int Duration,
    string DurationPeriod,
    bool IsActive
);
