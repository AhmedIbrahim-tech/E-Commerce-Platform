namespace Application.ServicesHandlers.Services;

public interface IUserStatusService
{
    Task<bool> IsUserDeletedAsync(Guid userId, CancellationToken cancellationToken = default);
}
