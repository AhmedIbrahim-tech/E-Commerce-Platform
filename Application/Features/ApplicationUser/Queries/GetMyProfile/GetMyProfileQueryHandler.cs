using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Domain.Entities.AuditLogs;
using Domain.Enums;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using System.Text.Json;

namespace Application.Features.ApplicationUser.Queries.GetMyProfile;

public class GetMyProfileQueryHandler(
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetMyProfileQuery, ApiResponse<GetMyProfileResponse>>
{
    public async Task<ApiResponse<GetMyProfileResponse>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated)
        {
            return new ApiResponse<GetMyProfileResponse>(UserErrors.InvalidCredentials());
        }

        var userId = currentUserService.GetUserId();
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return new ApiResponse<GetMyProfileResponse>(UserErrors.UserNotFound());
        }

        var roles = await userManager.GetRolesAsync(user);

        var accountType = roles.FirstOrDefault() ?? string.Empty;
        var accountStatus = user.LockoutEnd.HasValue && user.LockoutEnd.Value.UtcDateTime > DateTime.UtcNow
            ? "Locked"
            : "Active";

        var ordersQuery = unitOfWork.Orders.GetTableNoTracking()
            .Where(o => o.CustomerId == user.Id);

        var totalOrders = await ordersQuery.CountAsync(cancellationToken);
        var completedOrders = await ordersQuery.CountAsync(o => o.Status == Status.Completed || o.Status == Status.Received, cancellationToken);
        var pendingOrders = await ordersQuery.CountAsync(o => o.Status == Status.Pending || o.Status == Status.Paid || o.Status == Status.Draft, cancellationToken);
        var totalSpent = await ordersQuery
                             .Where(o => o.Status == Status.Paid || o.Status == Status.Completed || o.Status == Status.Received)
                             .Select(o => (decimal?)o.TotalAmount)
                             .SumAsync(cancellationToken) ?? 0m;

        var recentAuditLogs = await unitOfWork.AuditLogs.GetTableNoTracking()
            .Where(l => l.UserId == user.Id)
            .OrderByDescending(l => l.CreatedTime)
            .Take(10)
            .ToListAsync(cancellationToken);

        static string ResolveRelatedEntity(AuditLog log)
        {
            if (!string.IsNullOrWhiteSpace(log.AdditionalData))
            {
                try
                {
                    using var doc = JsonDocument.Parse(log.AdditionalData);
                    if (doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        if (doc.RootElement.TryGetProperty("entityType", out var entityTypeProp) &&
                            entityTypeProp.ValueKind == JsonValueKind.String)
                        {
                            return entityTypeProp.GetString() ?? log.EventType;
                        }

                        if (doc.RootElement.TryGetProperty("entity", out var entityProp) &&
                            entityProp.ValueKind == JsonValueKind.String)
                        {
                            return entityProp.GetString() ?? log.EventType;
                        }
                    }
                }
                catch
                {
                    return log.EventType;
                }
            }

            return log.EventType;
        }

        var response = new GetMyProfileResponse
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            DisplayName = user.DisplayName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            ProfileImageUrl = fileUploadService.ToAbsoluteUrl(user.ProfileImage),
            Roles = roles.ToList(),
            AccountType = accountType,
            AccountStatus = accountStatus,
            EcommerceStats = new EcommerceStatsResponse
            {
                TotalOrders = totalOrders,
                CompletedOrders = completedOrders,
                PendingOrders = pendingOrders,
                TotalSpent = totalSpent
            },
            RecentActivities = recentAuditLogs.Select(l => new ActivityItemResponse
            {
                Id = l.Id,
                ActionType = l.EventName,
                RelatedEntity = ResolveRelatedEntity(l),
                Description = l.Description,
                CreatedAt = l.CreatedTime
            }).ToList()
        };

        return Success(response);
    }
}

