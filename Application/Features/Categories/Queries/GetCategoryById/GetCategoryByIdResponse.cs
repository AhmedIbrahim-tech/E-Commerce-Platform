namespace Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdResponse(Guid Id, string Name, string? Description);

