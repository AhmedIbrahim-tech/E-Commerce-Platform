namespace Application.Features.Tags.Queries.GetTagById;

public record GetTagByIdQuery(Guid Id) : IRequest<ApiResponse<GetTagByIdResponse>>;

