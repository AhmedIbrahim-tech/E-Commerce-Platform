namespace Application.Features.Documents.Commands.DeleteMyDocument;

public record DeleteMyDocumentCommand(Guid Id) : IRequest<ApiResponse<string>>;

