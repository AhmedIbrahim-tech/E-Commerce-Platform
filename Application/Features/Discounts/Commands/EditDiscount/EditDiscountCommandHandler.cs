using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Discounts.Commands.EditDiscount;

public class EditDiscountCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditDiscountCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditDiscountCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
            return new ApiResponse<string>(DiscountErrors.InvalidDateRange());

        var discount = await unitOfWork.Discounts.GetTableNoTracking()
            .Where(d => d.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (discount == null) return new ApiResponse<string>(DiscountErrors.DiscountNotFound());

        if (request.DiscountPlanId.HasValue)
        {
            var planExists = await unitOfWork.DiscountPlans.GetTableNoTracking()
                .AnyAsync(dp => dp.Id == request.DiscountPlanId.Value, cancellationToken);

            if (!planExists)
                return BadRequest<string>("Discount plan not found");
        }

        discount.Name = request.Name;
        discount.Description = request.Description;
        discount.DiscountAmount = request.DiscountAmount;
        discount.DiscountPercentage = request.DiscountPercentage;
        discount.StartDate = request.StartDate;
        discount.EndDate = request.EndDate;
        discount.DiscountPlanId = request.DiscountPlanId;
        discount.IsActive = request.IsActive;

        try
        {
            await unitOfWork.Discounts.UpdateAsync(discount, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(DiscountErrors.DuplicatedDiscountName());
        }
    }
}
