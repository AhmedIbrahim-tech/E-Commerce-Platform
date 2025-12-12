using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Payments.Commands.SetPaymentMethod;

public class SetPaymentMethodCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<SetPaymentMethodCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(SetPaymentMethodCommand request, CancellationToken cancellationToken)
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

        if (order.Payment == null)
            order.Payment = new Payment();

        order.Payment.PaymentMethod = request.PaymentMethod;
        order.Payment.Status = Status.Draft;

        try
        {
            await unitOfWork.Orders.UpdateAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(OrderErrors.InvalidOrderStatus());
        }
    }
}

