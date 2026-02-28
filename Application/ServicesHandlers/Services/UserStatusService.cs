namespace Application.ServicesHandlers.Services;

public class UserStatusService(IUnitOfWork unitOfWork) : IUserStatusService
{
    public async Task<bool> IsUserDeletedAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (from c in unitOfWork.Customers.GetTableNoTracking()
                      where c.AppUserId == userId && c.IsDeleted
                      select true)
            .Union(from v in unitOfWork.Vendors.GetTableNoTracking()
                   where v.AppUserId == userId && v.IsDeleted
                   select true)
            .Union(from a in unitOfWork.Admins.GetTableNoTracking()
                   where a.AppUserId == userId && a.IsDeleted
                   select true)
            .AnyAsync(cancellationToken);
    }
}
