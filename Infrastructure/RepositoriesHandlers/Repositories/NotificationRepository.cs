using Domain.Entities.Notifications;

namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface INotificationRepository : IGenericRepositoryAsync<Notification>
{
}

public class NotificationRepository(ApplicationDbContext dbContext)
    : GenericRepositoryAsync<Notification>(dbContext), INotificationRepository
{
    private readonly DbSet<Notification> _notifications = dbContext.Set<Notification>();
}

