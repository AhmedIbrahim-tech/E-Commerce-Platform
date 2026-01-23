using Domain.Entities.Users;

namespace Application.Wrappers;

public static class AdminQueryableExtensions
{
    public static IQueryable<Admin> ApplyFiltering(
        this IQueryable<Admin> query,
        AdminSortingEnum? sortBy,
        string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                (e.FullName != null && e.FullName.Contains(search)) ||
                (e.Address != null && e.Address.Contains(search)));
        }

        query = sortBy switch
        {
            AdminSortingEnum.FullNameAsc => query.OrderBy(e => e.FullName ?? string.Empty),
            AdminSortingEnum.FullNameDesc => query.OrderByDescending(e => e.FullName ?? string.Empty),
            AdminSortingEnum.AddressAsc => query.OrderBy(e => e.Address ?? string.Empty),
            AdminSortingEnum.AddressDesc => query.OrderByDescending(e => e.Address ?? string.Empty),
            _ => query.OrderBy(e => e.FullName ?? string.Empty)
        };
        return query;
    }
}
