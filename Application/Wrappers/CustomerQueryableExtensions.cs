using Domain.Entities.Users;
using Domain.Enums.Sorting;

namespace Application.Wrappers;

public static class CustomerQueryableExtensions
{
    public static IQueryable<Customer> ApplyFiltering(
        this IQueryable<Customer> query,
        CustomerSortingEnum? sortBy,
        string? search)
    {
        // Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                (e.FullName != null && e.FullName.Contains(search)));
        }

        // Sort
        query = sortBy switch
        {
            CustomerSortingEnum.FullNameAsc => query.OrderBy(e => e.FullName ?? string.Empty),
            CustomerSortingEnum.FullNameDesc => query.OrderByDescending(e => e.FullName ?? string.Empty),
            _ => query.OrderBy(e => e.FullName ?? string.Empty)
        };
        return query;
    }
}
