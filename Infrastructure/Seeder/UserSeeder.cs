using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, ApplicationDbContext dbContext)
        {
            var usersCount = await userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var appUser = new AppUser
                {
                    UserName = "ahmad.eprahim",
                    Email = "ebrahema89859@gmail.com",
                    PhoneNumber = "01007691743",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(appUser, "Ah7_med@123");
                await userManager.AddToRoleAsync(appUser, "Admin");

                var admin = new Admin
                {
                    Id = Guid.NewGuid(),
                    AppUserId = appUser.Id,
                    FullName = "Ahmad Eprahim",
                    Gender = Gender.Male,
                    Address = "Egypt, Mansoura, MitGhamr"
                };

                await dbContext.Admins.AddAsync(admin);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
