using Application.ServicesHandlers.Services;
using Domain.Entities.AuditLogs;
using Domain.Entities.Documents;
using System.Text.Json;

namespace Application.Features.Documents.Commands.UploadMyDocument;

public class UploadMyDocumentCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<UploadMyDocumentCommand, ApiResponse<UploadMyDocumentResponse>>
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf",
        ".png",
        ".jpg",
        ".jpeg",
        ".doc",
        ".docx"
    };

    public async Task<ApiResponse<UploadMyDocumentResponse>> Handle(UploadMyDocumentCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        if (string.IsNullOrWhiteSpace(request.Type))
            return BadRequest<UploadMyDocumentResponse>("Document type is required");

        if (request.File == null || request.File.Length == 0)
            return BadRequest<UploadMyDocumentResponse>("File is required");

        const long maxSizeBytes = 20 * 1024 * 1024;
        if (request.File.Length > maxSizeBytes)
            return BadRequest<UploadMyDocumentResponse>("File size exceeds the maximum allowed limit");

        var extension = Path.GetExtension(request.File.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            return BadRequest<UploadMyDocumentResponse>("Unsupported file type");

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        string? relativePath = null;

        try
        {
            var document = new Document(
                userId: userId,
                type: request.Type,
                fileName: Path.GetFileName(request.File.FileName),
                contentType: request.File.ContentType,
                size: request.File.Length);

            await unitOfWork.Documents.AddAsync(document, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var uploadedPaths = await fileUploadService.UploadAsync(
                new[] { request.File },
                FileLocations.Documents,
                document.Id,
                childFolder: null,
                overwrite: true,
                cancellationToken: cancellationToken);

            if (uploadedPaths.Count == 0)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return BadRequest<UploadMyDocumentResponse>("Failed to upload document");
            }

            relativePath = uploadedPaths[0];

            document.AttachFile(relativePath, userId);
            await unitOfWork.Documents.UpdateAsync(document, cancellationToken);

            unitOfWork.Context.AuditLogs.Add(new AuditLog(
                eventType: "Documents",
                eventName: "DocumentUploaded",
                description: $"Document {document.Id} uploaded",
                userId: userId,
                userEmail: null,
                additionalData: JsonSerializer.Serialize(new
                {
                    entityType = "document",
                    entityId = document.Id,
                    documentType = document.Type,
                    status = document.Status.ToString()
                })));

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            var response = new UploadMyDocumentResponse(
                document.Id,
                document.UserId,
                document.Type,
                document.Status,
                document.FileName,
                document.Size,
                document.CreatedTime);

            return Created(response, "Document uploaded successfully");
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            if (!string.IsNullOrWhiteSpace(relativePath))
                await fileUploadService.TryDeleteFileAsync(relativePath, cancellationToken);
            return BadRequest<UploadMyDocumentResponse>("Failed to upload document");
        }
    }
}

