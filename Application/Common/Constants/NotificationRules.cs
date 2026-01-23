namespace Application.Common.Constants;

public static class NotificationRules
{
    public static readonly IReadOnlyDictionary<string, NotificationEventRecipients> Events =
        new Dictionary<string, NotificationEventRecipients>(StringComparer.OrdinalIgnoreCase)
        {
            [NotificationEvents.NewUser] = new NotificationEventRecipients
            {
                SuperAdmin = true,
                Admin = true,
                Merchant = false,
                Customer = false
            },
            [NotificationEvents.NewOrder] = new NotificationEventRecipients
            {
                SuperAdmin = true,
                Admin = true,
                Merchant = true,
                Customer = false
            },
            [NotificationEvents.OrderStatusUpdated] = new NotificationEventRecipients
            {
                SuperAdmin = false,
                Admin = false,
                Merchant = true,
                Customer = true
            },
            [NotificationEvents.MerchantRegistered] = new NotificationEventRecipients
            {
                SuperAdmin = true,
                Admin = true,
                Merchant = true,
                Customer = false
            },
            [NotificationEvents.MerchantProductAdded] = new NotificationEventRecipients
            {
                SuperAdmin = false,
                Admin = true,
                Merchant = true,
                Customer = false
            },
            [NotificationEvents.MerchantOrderReceived] = new NotificationEventRecipients
            {
                SuperAdmin = true,
                Admin = true,
                Merchant = true,
                Customer = false
            }
        };
}

