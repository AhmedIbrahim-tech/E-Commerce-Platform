namespace Application.Features.Documents.Commands.UploadMyDocument;

public record UploadMyDocumentCommand : IRequest<ApiResponse<UploadMyDocumentResponse>>
{
    public string Type { get; init; } = string.Empty;
    public IFormFile File { get; init; } = null!;
}

