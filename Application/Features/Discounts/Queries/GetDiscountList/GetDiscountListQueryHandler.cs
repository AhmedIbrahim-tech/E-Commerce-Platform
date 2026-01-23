using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Discounts.Queries.GetDiscountList;

public class GetDiscountListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetDiscountListQuery, ApiResponse<List<GetDiscountListResponse>>>
{
    public async Task<ApiResponse<List<GetDiscountListResponse>>> Handle(GetDiscountListQuery request, CancellationToken cancellationToken)
    {
        var discountList = await unitOfWork.Discounts.GetTableNoTracking()
            .Include(d => d.DiscountPlan)
            .Select(d => new GetDiscountListResponse(
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
            .ToListAsync(cancellationToken);

        return Success(discountList);
    }
}
