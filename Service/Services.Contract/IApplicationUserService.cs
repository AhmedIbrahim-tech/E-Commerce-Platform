
namespace Service.Services.Contract
{
    public interface IApplicationUserService
    {
        Task<string> AddUserAsync(User user, string password);
    }
}
