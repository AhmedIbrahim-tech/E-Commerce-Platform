
namespace Service.AuthService.Services.Contract
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        Guid GetUserId();
        Guid GetCartOwnerId();
        Task<User> GetUserAsync();
        Task<List<string>> GetCurrentUserRolesAsync();
        bool DeleteGuestIdCookie();
    }
}
