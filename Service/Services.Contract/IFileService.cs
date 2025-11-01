
namespace Service.Services.Contract
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(string location, IFormFile file);
    }
}
