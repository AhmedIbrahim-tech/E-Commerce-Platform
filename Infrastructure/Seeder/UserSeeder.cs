using System.Security.Claims;

namespace Infrastructure.Seeder;

public static class UserSeeder
{
    public const string DefaultUsername = "ahmad.eprahim";
    public const string DefaultEmail = "ebrahema89859@gmail.com";

    public static async Task SeedAsync(UserManager<AppUser> userManager, ApplicationDbContext dbContext, IDefaultClaimsService defaultClaimsService)
    {
        var usersCount = await userManager.Users.CountAsync();
        if (usersCount <= 0)
        {
            var displayName = "Ahmad Eprahim";
            var appUser = new AppUser(DefaultUsername, displayName)
            {
                Email = DefaultEmail,
                PhoneNumber = "01007691743",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(appUser, "Ah7_med@123");
            if (!result.Succeeded)
                throw new Exception($"Failed to create default user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            var createdUser = await userManager.FindByNameAsync(DefaultUsername);
            if (createdUser == null)
                throw new Exception("Failed to retrieve created user");

            await userManager.AddToRoleAsync(createdUser, Roles.SuperAdmin);
            await userManager.AddToRoleAsync(createdUser, Roles.Admin);

            var allClaims = Permissions.GetAll();
            var claims = allClaims.Select(claim => new Claim(claim, "True")).ToList();
            await userManager.AddClaimsAsync(createdUser, claims);

            var admin = new Admin(
                appUserId: createdUser.Id,
                fullName: displayName,
                gender: Gender.Male,
                address: "Egypt, Mansoura, MitGhamr",
                createdBy: createdUser.Id
            );

            await dbContext.Admins.AddAsync(admin);
            await dbContext.SaveChangesAsync();

            var adminUsername = "admin";
            var adminEmail = "admin@tajerly.com";
            var adminDisplayName = "Admin User";
            var adminAppUser = new AppUser(adminUsername, adminDisplayName)
            {
                Email = adminEmail,
                PhoneNumber = "01000000001",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var adminResult = await userManager.CreateAsync(adminAppUser, "Admin@123");
            if (!adminResult.Succeeded)
                throw new Exception($"Failed to create admin user: {string.Join(", ", adminResult.Errors.Select(e => e.Description))}");

            var createdAdminUser = await userManager.FindByNameAsync(adminUsername);
            if (createdAdminUser == null)
                throw new Exception("Failed to retrieve created admin user");

            await userManager.AddToRoleAsync(createdAdminUser, Roles.Admin);

            var adminProfile = new Admin(
                appUserId: createdAdminUser.Id,
                fullName: adminDisplayName,
                gender: Gender.Male,
                address: "Egypt, Cairo",
                createdBy: createdUser.Id
            );

            await dbContext.Admins.AddAsync(adminProfile);

            var customerUsername = "customer";
            var customerEmail = "customer@tajerly.com";
            var customerDisplayName = "Customer User";
            var customerAppUser = new AppUser(customerUsername, customerDisplayName)
            {
                Email = customerEmail,
                PhoneNumber = "01000000002",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var customerResult = await userManager.CreateAsync(customerAppUser, "Customer@123");
            if (!customerResult.Succeeded)
                throw new Exception($"Failed to create customer user: {string.Join(", ", customerResult.Errors.Select(e => e.Description))}");

            var createdCustomerUser = await userManager.FindByNameAsync(customerUsername);
            if (createdCustomerUser == null)
                throw new Exception("Failed to retrieve created customer user");

            await userManager.AddToRoleAsync(createdCustomerUser, Roles.Customer);

            var customerProfile = new Customer(
                appUserId: createdCustomerUser.Id,
                fullName: customerDisplayName,
                gender: Gender.Male,
                createdBy: createdUser.Id
            );

            await dbContext.Customers.AddAsync(customerProfile);

            var merchantUsername = "merchant";
            var merchantEmail = "merchant@tajerly.com";
            var merchantDisplayName = "Merchant User";
            var merchantAppUser = new AppUser(merchantUsername, merchantDisplayName)
            {
                Email = merchantEmail,
                PhoneNumber = "01000000003",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var merchantResult = await userManager.CreateAsync(merchantAppUser, "Merchant@123");
            if (!merchantResult.Succeeded)
                throw new Exception($"Failed to create merchant user: {string.Join(", ", merchantResult.Errors.Select(e => e.Description))}");

            var createdMerchantUser = await userManager.FindByNameAsync(merchantUsername);
            if (createdMerchantUser == null)
                throw new Exception("Failed to retrieve created merchant user");

            await userManager.AddToRoleAsync(createdMerchantUser, Roles.Merchant);

            var vendorProfile = new Vendor(
                appUserId: createdMerchantUser.Id,
                ownerName: merchantDisplayName,
                gender: Gender.Male,
                storeName: "Merchant Store",
                commissionRate: 10,
                createdBy: createdUser.Id
            );

            await dbContext.Vendors.AddAsync(vendorProfile);
            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task SyncAllClaimsToDefaultUserAsync(UserManager<AppUser> userManager)
    {
        var defaultUser = await userManager.FindByNameAsync(DefaultUsername) ?? await userManager.FindByEmailAsync(DefaultEmail);

        if (defaultUser == null)
            return;

        var allClaims = Permissions.GetAll();

        var currentClaims = await userManager.GetClaimsAsync(defaultUser);
        var currentClaimTypes = currentClaims.Select(c => c.Type).ToHashSet();

        var missingClaims = allClaims
            .Where(claim => !currentClaimTypes.Contains(claim))
            .Select(claim => new Claim(claim, "True"))
            .ToList();

        if (missingClaims.Count != 0)
        {
            await userManager.AddClaimsAsync(defaultUser, missingClaims);
        }
    }
}
