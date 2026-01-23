namespace Application.Common.DTOs;

/// <summary>
/// Base lookup DTO with common properties for all lookup types
/// </summary>
public record BaseLookupDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}


/// <summary>
/// Role lookup DTO (doesn't have Id, uses Name as identifier)
/// </summary>
public record RoleLookupDto
{
    public string Name { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
}

public record EnumLookupDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}


public record GetSubCategoriesByCategoryRequest(Guid CategoryId);