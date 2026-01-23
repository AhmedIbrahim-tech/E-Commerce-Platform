using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetBrandByIdQuery, ApiResponse<GetBrandByIdResponse>>
{
    public async Task<ApiResponse<GetBrandByIdResponse>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await unitOfWork.Brands.GetTableNoTracking()
            .Where(b => b.Id == request.Id)
            .Select(b => new GetBrandByIdResponse(
                b.Id,
                b.Name,
                b.Description,
                b.ImageUrl,
                b.IsActive,
                b.CreatedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (brand == null)
            return NotFound<GetBrandByIdResponse>("Brand not found");

        return Success(brand);
    }
}
