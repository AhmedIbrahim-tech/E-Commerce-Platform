using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetDiscountByIdQuery, ApiResponse<GetDiscountByIdResponse>>
{
    public async Task<ApiResponse<GetDiscountByIdResponse>> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        var discount = await unitOfWork.Discounts.GetTableNoTracking()
            .Include(d => d.DiscountPlan)
            .Where(d => d.Id == request.Id)
            .Select(d => new GetDiscountByIdResponse(
                d.Id,
                d.Name,
                d.Description,
                d.DiscountAmount,
                d.DiscountPercentage,
                d.StartDate,
                d.EndDate,
                d.DiscountPlanId,
                d.DiscountPlan != null ? d.DiscountPlan.Name : null,
                d.IsActive,
                d.CreatedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (discount == null)
            return NotFound<GetDiscountByIdResponse>("Discount not found");

        return Success(discount);
    }
}
