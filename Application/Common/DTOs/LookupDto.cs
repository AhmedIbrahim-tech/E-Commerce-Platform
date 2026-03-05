namespace Application.Common.DTOs;


public record BaseLookupDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}


public record RoleLookupDto : BaseLookupDto
{
    public string DisplayName { get; init; } = string.Empty;
}

public record EnumLookupDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}


public record GetSubCategoriesByCategoryRequest(Guid CategoryId);