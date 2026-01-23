using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.SubCategories.Queries.GetSubCategoryById;

public class GetSubCategoryByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetSubCategoryByIdQuery, ApiResponse<GetSubCategoryByIdResponse>>
{
    public async Task<ApiResponse<GetSubCategoryByIdResponse>> Handle(GetSubCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var subCategory = await unitOfWork.SubCategories.GetTableNoTracking()
            .Include(sc => sc.Category)
            .Where(sc => sc.Id == request.Id)
            .Select(sc => new GetSubCategoryByIdResponse(
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
            .FirstOrDefaultAsync(cancellationToken);

        if (subCategory == null)
            return NotFound<GetSubCategoryByIdResponse>("Sub category not found");

        return Success(subCategory);
    }
}
