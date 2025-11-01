
namespace Core.Features.Products.Queries.Responses
{
    public record GetSingleProductResponse(
        Guid Id,
        string? Name,
        string? Description,
        decimal? Price,
        int? StockQuantity,
        string? ImageURL,
        DateTimeOffset? CreatedAt,
        string? CategoryName)
    {
        public PaginatedResult<ReviewResponse>? Reviews { get; set; }
    }

    public record ReviewResponse(
        Guid CustomerId,
        string? FullName,
        Rating? Rating,
        string? Comment);
}
