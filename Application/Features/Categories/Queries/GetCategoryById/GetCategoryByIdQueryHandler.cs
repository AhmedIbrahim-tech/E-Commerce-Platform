using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetCategoryByIdQuery, ApiResponse<GetCategoryByIdResponse>>
{
    public async Task<ApiResponse<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Categories.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null) return new ApiResponse<GetCategoryByIdResponse>(CategoryErrors.CategoryNotFound());

        var categoryResponse = new GetCategoryByIdResponse(
            category.Id,
            category.Name!,
            category.Description
        );

        return Success(categoryResponse);
    }
}

