using Application.Common.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.ServicesHandlers.Services;

public interface IFileService
{
    Task<string> UploadImageAsync(string location, IFormFile file, CancellationToken cancellationToken = default);
    Task<string?> UploadImageAndGetFullUrlAsync(IFormFile? file, string location, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken = default);

    Task<string> UploadFileAsync(
        string moduleLocation,
        Guid recordId,
        IFormFile file,
        string? fileNameWithoutExtension = null,
        string? subFolder = null,
        bool overwrite = true,
        CancellationToken cancellationToken = default);

    Task<string?> UploadFileAndGetFullUrlAsync(
        IFormFile? file,
        string moduleLocation,
        Guid recordId,
        IHttpContextAccessor httpContextAccessor,
        string? fileNameWithoutExtension = null,
        string? subFolder = null,
        bool overwrite = true,
        CancellationToken cancellationToken = default);

    Task TryDeleteFileAsync(string? relativeOrAbsoluteUrl, CancellationToken cancellationToken = default);
    string? ToAbsoluteUrl(string? relativeOrAbsoluteUrl, IHttpContextAccessor httpContextAccessor);
}

public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<string> UploadFileAsync(
        string moduleLocation,
        Guid recordId,
        IFormFile file,
        string? fileNameWithoutExtension = null,
        string? subFolder = null,
        bool overwrite = true,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(moduleLocation))
            throw new ArgumentException("Module location cannot be null or empty.", nameof(moduleLocation));

        if (file == null)
            throw new ArgumentNullException(nameof(file), "File cannot be null.");

        if (file.Length == 0)
            return "NoFile";

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("File must have a valid extension.", nameof(file));

        var safeModuleLocation = moduleLocation.Trim().TrimStart('/').Replace('\\', '/');
        var safeRecordId = recordId.ToString();
        var safeSubFolder = string.IsNullOrWhiteSpace(subFolder) ? null : subFolder.Trim().Trim('/').Replace('\\', '/');

        var directoryPath = safeSubFolder == null
            ? Path.Combine(_webHostEnvironment.WebRootPath, safeModuleLocation, safeRecordId)
            : Path.Combine(_webHostEnvironment.WebRootPath, safeModuleLocation, safeRecordId, safeSubFolder);

        var baseName = string.IsNullOrWhiteSpace(fileNameWithoutExtension)
            ? $"{Guid.NewGuid():N}"
            : fileNameWithoutExtension.Trim();

        var fileName = $"{baseName}{extension}";
        var filePath = Path.Combine(directoryPath, fileName);

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (overwrite && File.Exists(filePath))
                File.Delete(filePath);

            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(fileStream, cancellationToken);
            await fileStream.FlushAsync(cancellationToken);

            var relativeParts = safeSubFolder == null
                ? new[] { safeModuleLocation, safeRecordId, fileName }
                : new[] { safeModuleLocation, safeRecordId, safeSubFolder, fileName };

            var relativePath = "/" + string.Join("/", relativeParts);
            return relativePath;
        }
        catch (DirectoryNotFoundException)
        {
            return "FailedToUploadFile";
        }
        catch (UnauthorizedAccessException)
        {
            return "FailedToUploadFile";
        }
        catch (IOException)
        {
            return "FailedToUploadFile";
        }
        catch (Exception)
        {
            return "FailedToUploadFile";
        }
    }

    public async Task<string?> UploadFileAndGetFullUrlAsync(
        IFormFile? file,
        string moduleLocation,
        Guid recordId,
        IHttpContextAccessor httpContextAccessor,
        string? fileNameWithoutExtension = null,
        string? subFolder = null,
        bool overwrite = true,
        CancellationToken cancellationToken = default)
    {
        if (file == null)
            return null;

        var context = httpContextAccessor.HttpContext?.Request;
        if (context == null)
            return null;

        var baseUrl = $"{context.Scheme}://{context.Host}";
        var fileUrl = await UploadFileAsync(moduleLocation, recordId, file, fileNameWithoutExtension, subFolder, overwrite, cancellationToken);

        if (fileUrl == "FailedToUploadFile" || fileUrl == "NoFile")
            return null;

        return baseUrl + fileUrl;
    }

    public async Task TryDeleteFileAsync(string? relativeOrAbsoluteUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativeOrAbsoluteUrl))
            return;

        string? relativePath = null;

        if (Uri.TryCreate(relativeOrAbsoluteUrl, UriKind.Absolute, out var absoluteUri))
        {
            relativePath = absoluteUri.AbsolutePath;
        }
        else
        {
            relativePath = relativeOrAbsoluteUrl;
        }

        relativePath = relativePath.Trim();
        if (relativePath.StartsWith("/"))
            relativePath = relativePath[1..];

        relativePath = relativePath.Replace('\\', '/');

        if (!relativePath.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            return;

        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));

        try
        {
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
        catch
        {
            await Task.CompletedTask;
        }
    }

    public string? ToAbsoluteUrl(string? relativeOrAbsoluteUrl, IHttpContextAccessor httpContextAccessor)
    {
        if (string.IsNullOrWhiteSpace(relativeOrAbsoluteUrl))
            return null;

        if (Uri.TryCreate(relativeOrAbsoluteUrl, UriKind.Absolute, out _))
            return relativeOrAbsoluteUrl;

        var context = httpContextAccessor.HttpContext?.Request;
        if (context == null)
            return relativeOrAbsoluteUrl;

        var baseUrl = $"{context.Scheme}://{context.Host}";
        var rel = relativeOrAbsoluteUrl.StartsWith("/") ? relativeOrAbsoluteUrl : "/" + relativeOrAbsoluteUrl;
        return baseUrl + rel;
    }

    public async Task<string> UploadImageAsync(string location, IFormFile file, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be null or empty.", nameof(location));

        if (file == null)
            throw new ArgumentNullException(nameof(file), "File cannot be null.");

        if (file.Length == 0)
            return "NoImage";

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("File must have a valid extension.", nameof(file));

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, location);
        var filePath = Path.Combine(directoryPath, fileName);

        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream, cancellationToken);
            await fileStream.FlushAsync(cancellationToken);

            var relativePath = $"/{location}/{fileName}";
            return relativePath;
        }
        catch (DirectoryNotFoundException)
        {
            return "FailedToUploadImage";
        }
        catch (UnauthorizedAccessException)
        {
            return "FailedToUploadImage";
        }
        catch (IOException)
        {
            return "FailedToUploadImage";
        }
        catch (Exception)
        {
            return "FailedToUploadImage";
        }
    }

    public async Task<string?> UploadImageAndGetFullUrlAsync(IFormFile? file, string location, IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken = default)
    {
        if (file == null)
            return null;

        var context = httpContextAccessor.HttpContext?.Request;
        if (context == null)
            return null;

        var baseUrl = $"{context.Scheme}://{context.Host}";
        var imageUrl = await UploadImageAsync(location, file, cancellationToken);

        if (imageUrl == "FailedToUploadImage" || imageUrl == "NoImage")
            return null;

        return baseUrl + imageUrl;
    }
}

