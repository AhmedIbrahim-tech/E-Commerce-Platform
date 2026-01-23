namespace Application.Features.Tags.Commands.DeleteTag;

public record DeleteTagCommand(Guid Id) : IRequest<ApiResponse<string>>;

