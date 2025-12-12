using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.ShippingAddresses.Commands.SetShippingAddress;

public class SetShippingAddressCommandHandler(
    IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<SetShippingAddressCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(SetShippingAddressCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetTableAsTracking()
            .Where(c => c.Id.Equals(request.OrderId))
            .Include(c => c.Customer)
            .Include(c => c.ShippingAddress)
            .Include(c => c.Payment)
            .Include(c => c.Delivery)
            .FirstOrDefaultAsync(cancellationToken);
        if (order == null)
            return new ApiResponse<string>(OrderErrors.OrderNotFound());

        if (order.Status != Status.Draft)
            return new ApiResponse<string>(OrderErrors.InvalidOrderStatus());

        var shippingAddress = await unitOfWork.ShippingAddresses.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.ShippingAddressId))
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(cancellationToken);

        if (shippingAddress == null)
            return new ApiResponse<string>(ShippingAddressErrors.ShippingAddressNotFound());

        var deliveryOffset = DeliveryTimeCalculator.Calculate(shippingAddress.City, order.Delivery!.DeliveryMethod);
        var deliveryCost = DeliveryCostCalculator.Calculate(shippingAddress.City, order.Delivery.DeliveryMethod);

        order.ShippingAddressId = request.ShippingAddressId;
        order.Delivery.Description = $"Delivery for order #{order.Id} to {shippingAddress.State}, {shippingAddress.City}, {shippingAddress.Street}";
        order.Delivery.DeliveryTime = DateTime.UtcNow.Add(deliveryOffset);
        order.Delivery.Cost = deliveryCost;

        try
        {
            await unitOfWork.Orders.UpdateAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ShippingAddressErrors.InvalidAddress());
        }
    }
}

