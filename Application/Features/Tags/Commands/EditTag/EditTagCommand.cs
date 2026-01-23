namespace Application.Features.Tags.Commands.EditTag;

public record EditTagCommand(Guid Id, string Name, bool IsActive) : IRequest<ApiResponse<string>>;

