
namespace Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            var usersCount = await userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defualtAdmin = new Admin()
                {
                    FirstName = "Ahmad",
                    LastName = "Eprahim",
                    UserName = "ahmad.eprahim",
                    Gender = Gender.Male,
                    Email = "ebrahema89859@gmail.com",
                    PhoneNumber = "01007691743",
                    Address = "Egypt, Mansoura, MitGhamr",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(defualtAdmin, "Ah7_med$#@!");
                await userManager.AddToRoleAsync(defualtAdmin, "Admin");
            }
        }
    }
}
