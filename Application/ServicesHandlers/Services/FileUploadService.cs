using Application.ServicesHandlers.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.ServicesHandlers.Services;

public interface IFileUploadService
{
    /// <summary>
    /// Uploads one or more files and returns relative paths.
    /// </summary>
    /// <param name="files">Files to upload. Can be a single file wrapped in an array.</param>
    /// <param name="baseFolder">Base folder name (e.g., "uploads", "users", "products")</param>
    /// <param name="entityId">Entity identifier (e.g., user ID, product ID)</param>
    /// <param name="childFolder">Optional subfolder within entity folder (e.g., "gallery", "invoices")</param>
    /// <param name="overwrite">If true, replaces existing file. If false, appends suffix to make unique.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of relative paths (e.g., "/uploads/users/{entityId}/avatar.jpg"). Empty list if no files provided.</returns>
    Task<IReadOnlyList<string>> UploadAsync(
        IEnumerable<IFormFile>? files,
        string baseFolder,
        Guid entityId,
        string? childFolder = null,
        bool overwrite = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads one or more files and returns full absolute URLs.
    /// </summary>
    /// <param name="files">Files to upload. Can be a single file wrapped in an array.</param>
    /// <param name="baseFolder">Base folder name (e.g., "uploads", "users", "products")</param>
    /// <param name="entityId">Entity identifier (e.g., user ID, product ID)</param>
    /// <param name="childFolder">Optional subfolder within entity folder (e.g., "gallery", "invoices")</param>
    /// <param name="overwrite">If true, replaces existing file. If false, appends suffix to make unique.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of full URLs (e.g., "https://example.com/uploads/users/{entityId}/avatar.jpg"). Empty list if no files provided.</returns>
    Task<IReadOnlyList<string>> UploadAndGetUrlsAsync(
        IEnumerable<IFormFile>? files,
        string baseFolder,
        Guid entityId,
        string? childFolder = null,
        bool overwrite = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file by relative or absolute URL. Only deletes files within allowed base folders (e.g., "uploads/") to prevent path traversal.
    /// </summary>
    /// <param name="relativeOrAbsoluteUrl">Relative path (e.g., "/uploads/users/123/file.jpg") or absolute URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task TryDeleteFileAsync(string? relativeOrAbsoluteUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Converts a relative path to an absolute URL using the current request base URL.
    /// </summary>
    /// <param name="relativeOrAbsoluteUrl">Relative path or absolute URL</param>
    /// <returns>Absolute URL if conversion successful, otherwise returns input unchanged or null</returns>
    string? ToAbsoluteUrl(string? relativeOrAbsoluteUrl);
}

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICurrentUserService _currentUserService;

    public FileUploadService(IWebHostEnvironment webHostEnvironment, ICurrentUserService currentUserService)
    {
        _webHostEnvironment = webHostEnvironment;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<string>> UploadAsync(
        IEnumerable<IFormFile>? files,
        string baseFolder,
        Guid entityId,
        string? childFolder = null,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        if (files == null)
            return Array.Empty<string>();

        var fileList = files.Where(f => f != null && f.Length > 0).ToList();
        if (fileList.Count == 0)
            return Array.Empty<string>();

        if (string.IsNullOrWhiteSpace(baseFolder))
            throw new ArgumentException("Base folder cannot be null or empty.", nameof(baseFolder));

        var results = new List<string>();

        foreach (var file in fileList)
        {
            try
            {
                var relativePath = await UploadSingleFileAsync(
                    file,
                    baseFolder,
                    entityId,
                    childFolder,
                    overwrite,
                    cancellationToken);

                if (!string.IsNullOrWhiteSpace(relativePath))
                    results.Add(relativePath);
            }
            catch
            {
                // Skip failed files, continue with others
            }
        }

        return results;
    }

    public async Task<IReadOnlyList<string>> UploadAndGetUrlsAsync(
        IEnumerable<IFormFile>? files,
        string baseFolder,
        Guid entityId,
        string? childFolder = null,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        var relativePaths = await UploadAsync(files, baseFolder, entityId, childFolder, overwrite, cancellationToken);

        var baseUrl = _currentUserService.GetBaseUrl();
        if (string.IsNullOrWhiteSpace(baseUrl))
            return relativePaths; // Return relative paths if base URL unavailable

        return relativePaths
            .Select(path => baseUrl + (path.StartsWith("/") ? path : "/" + path))
            .ToList();
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

        // Security: Only allow deletion within uploads folder
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

    public string? ToAbsoluteUrl(string? relativeOrAbsoluteUrl)
    {
        if (string.IsNullOrWhiteSpace(relativeOrAbsoluteUrl))
            return null;

        if (Uri.TryCreate(relativeOrAbsoluteUrl, UriKind.Absolute, out _))
        {
            var absoluteUri = new Uri(relativeOrAbsoluteUrl);
            var relativePath = absoluteUri.AbsolutePath;
            if (relativePath.StartsWith("/"))
                relativePath = relativePath[1..];
            
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath))
                return null;
            
            return relativeOrAbsoluteUrl;
        }

        var baseUrl = _currentUserService.GetBaseUrl();
        if (string.IsNullOrWhiteSpace(baseUrl))
            return null;

        var rel = relativeOrAbsoluteUrl.StartsWith("/") ? relativeOrAbsoluteUrl : "/" + relativeOrAbsoluteUrl;
        
        var relativePathForCheck = rel.TrimStart('/');
        var fullPathForCheck = Path.Combine(_webHostEnvironment.WebRootPath, relativePathForCheck.Replace('/', Path.DirectorySeparatorChar));
        
        if (!File.Exists(fullPathForCheck))
            return null;

        return baseUrl + rel;
    }

    private async Task<string> UploadSingleFileAsync(
        IFormFile file,
        string baseFolder,
        Guid entityId,
        string? childFolder,
        bool overwrite,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("File must have a valid extension.", nameof(file));

        // Sanitize and prepare filename
        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
        var sanitizedFileName = SanitizeFileName(originalFileName);

        // If sanitization resulted in empty name or contains non-ASCII, use fallback
        if (string.IsNullOrWhiteSpace(sanitizedFileName) || ContainsNonAscii(sanitizedFileName))
        {
            sanitizedFileName = $"file_{DateTimeOffset.UtcNow:yyyyMMddHHmmss}";
        }

        // Build directory path
        var safeBaseFolder = baseFolder.Trim().TrimStart('/').Replace('\\', '/');
        var safeEntityId = entityId.ToString();
        var safeChildFolder = string.IsNullOrWhiteSpace(childFolder) ? null : childFolder.Trim().Trim('/').Replace('\\', '/');

        var directoryPath = safeChildFolder == null
            ? Path.Combine(_webHostEnvironment.WebRootPath, safeBaseFolder, safeEntityId)
            : Path.Combine(_webHostEnvironment.WebRootPath, safeBaseFolder, safeEntityId, safeChildFolder);

        // Ensure directory exists
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        // Determine final filename with conflict resolution
        var finalFileName = DetermineFinalFileName(directoryPath, sanitizedFileName, extension, overwrite);
        var filePath = Path.Combine(directoryPath, finalFileName);

        // Upload file
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await file.CopyToAsync(fileStream, cancellationToken);
        await fileStream.FlushAsync(cancellationToken);

        // Build relative path
        var relativeParts = safeChildFolder == null
            ? new[] { safeBaseFolder, safeEntityId, finalFileName }
            : new[] { safeBaseFolder, safeEntityId, safeChildFolder, finalFileName };

        return "/" + string.Join("/", relativeParts);
    }

    private static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return string.Empty;

        // Remove invalid file system characters
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(fileName
            .Where(c => !invalidChars.Contains(c))
            .ToArray());

        // Normalize spaces and trim
        sanitized = Regex.Replace(sanitized, @"\s+", " ").Trim();

        // Remove leading/trailing dots and spaces (Windows restriction)
        sanitized = sanitized.Trim('.', ' ');

        return sanitized;
    }

    private static bool ContainsNonAscii(string text)
    {
        return text.Any(c => c > 127);
    }

    private static string DetermineFinalFileName(string directoryPath, string baseName, string extension, bool overwrite)
    {
        var fileName = $"{baseName}{extension}";
        var filePath = Path.Combine(directoryPath, fileName);

        if (overwrite || !File.Exists(filePath))
            return fileName;

        // Append suffix to make unique
        int suffix = 1;
        string uniqueFileName;
        do
        {
            uniqueFileName = $"{baseName}-{suffix}{extension}";
            filePath = Path.Combine(directoryPath, uniqueFileName);
            suffix++;
        } while (File.Exists(filePath) && suffix < 10000); // Safety limit

        return uniqueFileName;
    }
}
