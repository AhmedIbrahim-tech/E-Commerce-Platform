using Microsoft.EntityFrameworkCore;

namespace Application.Wrappers
{
    public static class EmployeeQueryableExtensions
    {
        public static IQueryable<Employee> ApplyFiltering(
            this IQueryable<Employee> query,
            EmployeeSortingEnum? sortBy,
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
                EmployeeSortingEnum.FirstNameAsc => query.OrderBy(e => e.FullName ?? string.Empty),
                EmployeeSortingEnum.FirstNameDesc => query.OrderByDescending(e => e.FullName ?? string.Empty),
                EmployeeSortingEnum.LastNameAsc => query.OrderBy(e => e.FullName ?? string.Empty),
                EmployeeSortingEnum.LastNameDesc => query.OrderByDescending(e => e.FullName ?? string.Empty),
                EmployeeSortingEnum.SalaryAsc => query.OrderBy(e => e.Salary),
                EmployeeSortingEnum.SalaryDesc => query.OrderByDescending(e => e.Salary),
                EmployeeSortingEnum.HireDateAsc => query.OrderBy(e => e.HireDate),
                EmployeeSortingEnum.HireDateDesc => query.OrderByDescending(e => e.HireDate),
                _ => query.OrderBy(e => e.FullName ?? string.Empty)
            };

            return query;
        }
    }
}
