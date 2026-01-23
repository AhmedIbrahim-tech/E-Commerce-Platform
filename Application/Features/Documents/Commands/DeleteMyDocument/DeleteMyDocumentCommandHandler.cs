using Application.ServicesHandlers.Services;
using Domain.Entities.AuditLogs;
using Domain.Entities.Documents;
using System.Text.Json;

namespace Application.Features.Documents.Commands.DeleteMyDocument;

public class DeleteMyDocumentCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<DeleteMyDocumentCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteMyDocumentCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var document = await unitOfWork.Documents.GetTableAsTracking()
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (document == null)
            return NotFound<string>("Document not found");

        if (document.UserId != userId)
            return Unauthorized<string>("You do not have permission to delete this document");

        await fileUploadService.TryDeleteFileAsync(document.FilePath, cancellationToken);
        document.MarkDeleted(userId);
        await unitOfWork.Documents.UpdateAsync(document, cancellationToken);

        unitOfWork.Context.AuditLogs.Add(new AuditLog(
            eventType: "Documents",
            eventName: "DocumentDeleted",
            description: $"Document {document.Id} deleted",
            userId: userId,
            userEmail: null,
            additionalData: JsonSerializer.Serialize(new
            {
                entityType = "document",
                entityId = document.Id,
                documentType = document.Type
            })));

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Deleted<string>("Document deleted successfully");
    }
}

