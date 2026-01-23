using System.Text.Json;
using Domain.Entities.Notifications;

namespace Application.ServicesHandlers.Services;

public class NotificationService(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    INotificationSender? notificationSender = null) : INotificationService
{
    public async Task CreateAsync(string type,object? data,NotificationRecipients recipients,CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            return;
        }

        var eventKey = type.Trim().ToLowerInvariant();
        if (!Application.Common.Constants.NotificationRules.Events.TryGetValue(eventKey, out var eventRecipients))
        {
            return;
        }

        var payload = SerializeData(data);

        var notifications = new List<Notification>();

        if (eventRecipients.SuperAdmin)
        {
            var superAdminIds = recipients.SuperAdminIds?.Where(x => x != Guid.Empty).Distinct().ToList();
            if (superAdminIds == null || superAdminIds.Count == 0)
            {
                var superAdmins = await userManager.GetUsersInRoleAsync(Roles.SuperAdmin);
                superAdminIds = superAdmins.Select(x => x.Id).Distinct().ToList();
            }

            notifications.AddRange(superAdminIds.Select(id =>
                new Notification(eventKey, NotificationRecipientRole.SuperAdmin, id, payload)));
        }

        if (eventRecipients.Admin && recipients.AdminIds != null)
        {
            var adminIds = recipients.AdminIds.Where(x => x != Guid.Empty).Distinct();
            notifications.AddRange(adminIds.Select(id =>
                new Notification(eventKey, NotificationRecipientRole.Admin, id, payload)));
        }

        if (eventRecipients.Merchant && recipients.MerchantIds != null)
        {
            var merchantIds = recipients.MerchantIds.Where(x => x != Guid.Empty).Distinct();
            notifications.AddRange(merchantIds.Select(id =>
                new Notification(eventKey, NotificationRecipientRole.Merchant, id, payload)));
        }

        if (eventRecipients.Customer && recipients.CustomerIds != null)
        {
            var customerIds = recipients.CustomerIds.Where(x => x != Guid.Empty).Distinct();
            notifications.AddRange(customerIds.Select(id =>
                new Notification(eventKey, NotificationRecipientRole.Customer, id, payload)));
        }

        if (notifications.Count == 0)
        {
            return;
        }

        await dbContext.Notifications.AddRangeAsync(notifications, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (notificationSender != null)
        {
            foreach (var notification in notifications)
            {
                var serialized = SerializeForRealtime(notification);
                await notificationSender.SendToUserAsync(notification.RecipientId.ToString(), serialized);
            }
        }
    }

    private static string SerializeData(object? data)
    {
        if (data == null)
        {
            return "{}";
        }

        return JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private static string SerializeForRealtime(Notification notification)
    {
        JsonElement parsedData;
        try
        {
            parsedData = JsonSerializer.Deserialize<JsonElement>(notification.Data);
        }
        catch
        {
            parsedData = JsonSerializer.Deserialize<JsonElement>("{}");
        }

        return JsonSerializer.Serialize(new
        {
            id = notification.Id,
            type = notification.Type,
            data = parsedData,
            isRead = notification.IsRead,
            createdAt = notification.CreatedAt
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}

