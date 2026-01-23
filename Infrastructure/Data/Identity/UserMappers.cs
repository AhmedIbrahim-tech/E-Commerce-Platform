using Domain.Entities.Users;

namespace Infrastructure.Data.Identity;

public static class UserMappers
{
    public static void UpdateFromCustomer(this AppUser appUser, Customer customer)
    {
        if (appUser == null || customer == null)
            return;

        appUser.SetDisplayName(customer.FullName ?? string.Empty);
    }

    public static void UpdateFromAdmin(this AppUser appUser, Admin admin)
    {
        if (appUser == null || admin == null)
            return;

        appUser.SetDisplayName(admin.FullName ?? string.Empty);
    }

    public static void UpdateFromVendor(this AppUser appUser, Vendor vendor)
    {
        if (appUser == null || vendor == null)
            return;

        appUser.SetDisplayName(vendor.FullName ?? string.Empty);
    }
}

