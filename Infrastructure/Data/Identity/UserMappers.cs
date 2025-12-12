using Domain.Entities;

namespace Infrastructure.Data.Identity
{
    public static class UserMappers
    {
        public static Customer ToCustomer(this AppUser appUser, Guid customerId)
        {
            if (appUser == null)
                return null!;

            return new Customer
            {
                Id = customerId,
                AppUserId = appUser.Id,
                FullName = appUser.FullName
            };
        }

        public static Customer ToCustomer(this AppUser appUser, Customer existingCustomer)
        {
            if (appUser == null || existingCustomer == null)
                return null!;

            existingCustomer.AppUserId = appUser.Id;
            existingCustomer.FullName = appUser.FullName;
            return existingCustomer;
        }

        public static Employee ToEmployee(this AppUser appUser, Guid employeeId)
        {
            if (appUser == null)
                return null!;

            return new Employee
            {
                Id = employeeId,
                AppUserId = appUser.Id,
                FullName = appUser.FullName
            };
        }

        public static Employee ToEmployee(this AppUser appUser, Employee existingEmployee)
        {
            if (appUser == null || existingEmployee == null)
                return null!;

            existingEmployee.AppUserId = appUser.Id;
            existingEmployee.FullName = appUser.FullName;
            return existingEmployee;
        }

        public static Admin ToAdmin(this AppUser appUser, Guid adminId)
        {
            if (appUser == null)
                return null!;

            return new Admin
            {
                Id = adminId,
                AppUserId = appUser.Id,
                FullName = appUser.FullName
            };
        }

        public static Admin ToAdmin(this AppUser appUser, Admin existingAdmin)
        {
            if (appUser == null || existingAdmin == null)
                return null!;

            existingAdmin.AppUserId = appUser.Id;
            existingAdmin.FullName = appUser.FullName;
            return existingAdmin;
        }

        public static void UpdateFromCustomer(this AppUser appUser, Customer customer)
        {
            if (appUser == null || customer == null)
                return;

            // Only update allowed fields from domain entity to AppUser
            appUser.FullName = customer.FullName;
        }

        public static void UpdateFromEmployee(this AppUser appUser, Employee employee)
        {
            if (appUser == null || employee == null)
                return;

            // Only update allowed fields from domain entity to AppUser
            appUser.FullName = employee.FullName;
        }

        public static void UpdateFromAdmin(this AppUser appUser, Admin admin)
        {
            if (appUser == null || admin == null)
                return;

            // Only update allowed fields from domain entity to AppUser
            appUser.FullName = admin.FullName;
        }
    }
}

