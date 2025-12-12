using Infrastructure.Data.Identity;

namespace Application.Wrappers
{
    public static class UserManagerExtensions
    {
        public static async Task<bool> UserNameExistsAsync(
            this UserManager<AppUser> userManager, string userName, Guid currentUserId)
        {
            return await userManager.Users.AnyAsync(u => u.UserName == userName && u.Id != currentUserId);
        }

        public static async Task<bool> EmailExistsAsync(
            this UserManager<AppUser> userManager, string email, Guid currentUserId)
        {
            return await userManager.Users.AnyAsync(u => u.Email == email && u.Id != currentUserId);
        }
    }

}
