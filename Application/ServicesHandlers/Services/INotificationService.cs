namespace Application.ServicesHandlers.Services;

public record NotificationRecipients(
    IReadOnlyCollection<Guid>? SuperAdminIds = null,
    IReadOnlyCollection<Guid>? AdminIds = null,
    IReadOnlyCollection<Guid>? MerchantIds = null,
    IReadOnlyCollection<Guid>? CustomerIds = null
);

public interface INotificationService
{
    Task CreateAsync(
        string type,
        object? data,
        NotificationRecipients recipients,
        CancellationToken cancellationToken = default);
}

