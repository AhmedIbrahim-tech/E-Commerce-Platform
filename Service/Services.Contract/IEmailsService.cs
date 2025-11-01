
namespace Service.Services.Contract
{
    public interface IEmailsService
    {
        Task<string> SendEmailAsync(string email, string ReturnUrl, EmailType? emailType, Order? order = null);
    }
}
