using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.VariantAttributes.Queries.GetVariantAttributeList;

public class GetVariantAttributeListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetVariantAttributeListQuery, ApiResponse<List<GetVariantAttributeListResponse>>>
{
    public async Task<ApiResponse<List<GetVariantAttributeListResponse>>> Handle(GetVariantAttributeListQuery request, CancellationToken cancellationToken)
    {
        var variantAttributeList = await unitOfWork.VariantAttributes.GetTableNoTracking()
            .Where(va => va.IsActive)
            .Select(va => new GetVariantAttributeListResponse(
                va.Id,
                va.Name,
                va.Description,
                va.IsActive,
                va.CreatedTime
            ))
            .ToListAsync(cancellationToken);

        return Success(variantAttributeList);
    }
}
