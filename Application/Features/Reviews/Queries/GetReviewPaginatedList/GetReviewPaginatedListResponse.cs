namespace Application.Features.Reviews.Queries.GetReviewPaginatedList;

public record GetReviewPaginatedListResponse
(
    string? UserName,
    string? ProductName,
    Rating? Rating,
    string? Comment,
    DateTimeOffset? CreatedAt);

