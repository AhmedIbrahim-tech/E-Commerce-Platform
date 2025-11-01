namespace Core.Features.Categories.Queries.Response
{
    public record GetCategoryPaginatedListResponse(Guid Id, string Name, string? Description);
}
