namespace Application.Features.Warranties.Queries.GetWarrantyById;

public record GetWarrantyByIdResponse(
    Guid Id,
    string Name,
    string? Description,
    int Duration,
    string DurationPeriod,
    bool IsActive,
    DateTimeOffset CreatedTime,
    DateTimeOffset? ModifiedTime
);
