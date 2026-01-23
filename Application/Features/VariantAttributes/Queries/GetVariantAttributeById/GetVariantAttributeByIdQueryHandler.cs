using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.VariantAttributes.Queries.GetVariantAttributeById;

public class GetVariantAttributeByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetVariantAttributeByIdQuery, ApiResponse<GetVariantAttributeByIdResponse>>
{
    public async Task<ApiResponse<GetVariantAttributeByIdResponse>> Handle(GetVariantAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        var variantAttribute = await unitOfWork.VariantAttributes.GetTableNoTracking()
            .Where(va => va.Id == request.Id)
            .Select(va => new GetVariantAttributeByIdResponse(
                va.Id,
                va.Name,
                va.Description,
                va.IsActive,
                va.CreatedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (variantAttribute == null)
            return NotFound<GetVariantAttributeByIdResponse>("Variant attribute not found");

        return Success(variantAttribute);
    }
}
