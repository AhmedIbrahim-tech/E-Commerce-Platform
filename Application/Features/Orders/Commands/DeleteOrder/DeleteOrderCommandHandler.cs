using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteOrderCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.OrderId))
            .Include(c => c.Customer)
            .Include(c => c.ShippingAddress)
            .Include(c => c.Payment)
            .Include(c => c.Delivery)
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null) return new ApiResponse<string>(OrderErrors.OrderNotFound());

        try
        {
            await unitOfWork.Orders.DeleteAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            return new ApiResponse<string>(OrderErrors.CannotCancelOrder());
        }
    }
}

