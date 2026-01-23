using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.SubCategories.Queries.GetSubCategoryList;

public class GetSubCategoryListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetSubCategoryListQuery, ApiResponse<List<GetSubCategoryListResponse>>>
{
    public async Task<ApiResponse<List<GetSubCategoryListResponse>>> Handle(GetSubCategoryListQuery request, CancellationToken cancellationToken)
    {
        var queryable = unitOfWork.SubCategories.GetTableNoTracking()
            .Include(sc => sc.Category)
            .AsQueryable();

        if (request.CategoryId.HasValue)
            queryable = queryable.Where(sc => sc.CategoryId == request.CategoryId.Value);

        var subCategoryList = await queryable
            .Select(sc => new GetSubCategoryListResponse(
                sc.Id,
                sc.Name,
                sc.Description,
                sc.ImageUrl,
                sc.Code,
                sc.CategoryId,
                sc.Category != null ? sc.Category.Name : null,
                sc.IsActive,
                sc.CreatedTime
            ))
            .ToListAsync(cancellationToken);

        return Success(subCategoryList);
    }
}
