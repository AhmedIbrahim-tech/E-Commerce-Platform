namespace Infrastructure.Data.Authorization;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Merchant = "Merchant";
    public const string StaffMerchant = "StaffMerchant";
    public const string Customer = "Customer";

    public static List<string> GetAll()
    {
        return new List<string>
        {
            SuperAdmin,
            Admin,
            Merchant,
            StaffMerchant,
            Customer
        };
    }

    public static bool IsValid(string roleName)
    {
        return GetAll().Contains(roleName);
    }
}
