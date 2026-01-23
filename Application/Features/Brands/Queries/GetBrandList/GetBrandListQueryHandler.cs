using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Brands.Queries.GetBrandList;

public class GetBrandListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetBrandListQuery, ApiResponse<List<GetBrandListResponse>>>
{
    public async Task<ApiResponse<List<GetBrandListResponse>>> Handle(GetBrandListQuery request, CancellationToken cancellationToken)
    {
        var brandList = await unitOfWork.Brands.GetTableNoTracking()
            .Where(b => b.IsActive)
            .Select(b => new GetBrandListResponse(
                b.Id,
                b.Name,
                b.Description,
                b.ImageUrl,
                b.IsActive,
                b.CreatedTime
            ))
            .ToListAsync(cancellationToken);

        return Success(brandList);
    }
}
