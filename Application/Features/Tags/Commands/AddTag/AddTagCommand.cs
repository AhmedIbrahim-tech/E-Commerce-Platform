namespace Application.Features.Tags.Commands.AddTag;

public record AddTagCommand(string Name, bool IsActive = true) : IRequest<ApiResponse<string>>;

