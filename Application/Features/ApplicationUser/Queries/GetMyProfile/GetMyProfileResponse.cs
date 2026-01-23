namespace Application.Features.ApplicationUser.Queries.GetMyProfile;

public class GetMyProfileResponse
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string? ProfileImageUrl { get; init; }
    public List<string> Roles { get; init; } = [];

    public string AccountType { get; init; } = string.Empty;
    public string AccountStatus { get; init; } = string.Empty;

    public EcommerceStatsResponse EcommerceStats { get; init; } = new();
    public List<ActivityItemResponse> RecentActivities { get; init; } = [];
}

public class EcommerceStatsResponse
{
    public int TotalOrders { get; init; }
    public int CompletedOrders { get; init; }
    public int PendingOrders { get; init; }
    public decimal TotalSpent { get; init; }
}

public class ActivityItemResponse
{
    public Guid Id { get; init; }
    public string ActionType { get; init; } = string.Empty;
    public string RelatedEntity { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}

