namespace Application.Features.Discounts.Commands.AddDiscount;

public class AddDiscountCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddDiscountCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
            return new ApiResponse<string>(DiscountErrors.InvalidDateRange());

        if (request.DiscountPlanId.HasValue)
        {
            var planExists = await unitOfWork.DiscountPlans.GetTableNoTracking()
                .AnyAsync(dp => dp.Id == request.DiscountPlanId.Value, cancellationToken);

            if (!planExists)
                return BadRequest<string>("Discount plan not found");
        }

        try
        {
            var discount = new Discount
            {
                Name = request.Name,
                Description = request.Description,
                DiscountAmount = request.DiscountAmount,
                DiscountPercentage = request.DiscountPercentage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DiscountPlanId = request.DiscountPlanId,
                IsActive = request.IsActive
            };

            await unitOfWork.Discounts.AddAsync(discount, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(DiscountErrors.DuplicatedDiscountName());
        }
    }
}
