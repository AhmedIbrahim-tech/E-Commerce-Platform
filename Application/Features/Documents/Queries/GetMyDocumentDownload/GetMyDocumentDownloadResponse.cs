namespace Application.Features.Documents.Queries.GetMyDocumentDownload;

public class GetMyDocumentDownloadResponse
{
    public string RelativePath { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = "application/octet-stream";
}

