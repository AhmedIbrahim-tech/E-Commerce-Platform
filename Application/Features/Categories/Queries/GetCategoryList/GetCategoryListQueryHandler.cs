using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Categories.Queries.GetCategoryList;

public class GetCategoryListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetCategoryListQuery, ApiResponse<List<GetCategoryListResponse>>>
{
    public async Task<ApiResponse<List<GetCategoryListResponse>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var categoryList = await unitOfWork.Categories.GetAllAsync();
        var categoryListResponse = categoryList
            .Where(c => c != null)
            .Select(category => new GetCategoryListResponse(
                category!.Id,
                category.Name!,
                category.Description
            ))
            .ToList();

        return Success(categoryListResponse);
    }
}

