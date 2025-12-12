using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Deliveries.Commands.SetDeliveryMethod;

public class SetDeliveryMethodCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<SetDeliveryMethodCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(SetDeliveryMethodCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetTableAsTracking()
            .Where(c => c.Id.Equals(request.OrderId))
            .Include(c => c.Customer)
            .Include(c => c.ShippingAddress)
            .Include(c => c.Payment)
            .Include(c => c.Delivery)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null || order.Status != Status.Draft)
            return new ApiResponse<string>(OrderErrors.InvalidOrderStatus());

        if (order.Delivery == null)
            order.Delivery = new Delivery();

        order.Delivery.DeliveryMethod = request.DeliveryMethod;
        order.Delivery.Status = Status.Draft;

        try
        {
            await unitOfWork.Orders.UpdateAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(DeliveryErrors.CannotModifyDeliveryStatus());
        }
    }
}

