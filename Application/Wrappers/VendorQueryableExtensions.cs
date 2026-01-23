using Domain.Entities.Users;

namespace Application.Wrappers;

public static class VendorQueryableExtensions
{
    public static IQueryable<Vendor> ApplyFiltering(
        this IQueryable<Vendor> query,
        VendorSortingEnum? sortBy,
        string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                (e.FullName != null && e.FullName.Contains(search)) ||
                (e.StoreName != null && e.StoreName.Contains(search)));
        }

        query = sortBy switch
        {
            VendorSortingEnum.StoreNameAsc => query.OrderBy(e => e.StoreName ?? string.Empty),
            VendorSortingEnum.StoreNameDesc => query.OrderByDescending(e => e.StoreName ?? string.Empty),
            VendorSortingEnum.FullNameAsc => query.OrderBy(e => e.FullName ?? string.Empty),
            VendorSortingEnum.FullNameDesc => query.OrderByDescending(e => e.FullName ?? string.Empty),
            _ => query.OrderBy(e => e.StoreName ?? string.Empty)
        };
        return query;
    }
}
