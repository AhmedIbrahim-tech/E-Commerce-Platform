namespace Application.Features.ShippingAddresses.Commands.DeleteShippingAddress;

public class DeleteShippingAddressCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteShippingAddressCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteShippingAddressCommand request, CancellationToken cancellationToken)
    {
        var shippingAddress = await unitOfWork.ShippingAddresses.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.Id))
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(cancellationToken);

        if (shippingAddress == null) return new ApiResponse<string>(ShippingAddressErrors.ShippingAddressNotFound());

        try
        {
            await unitOfWork.ShippingAddresses.DeleteAsync(shippingAddress);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ShippingAddressErrors.CannotDeleteAddressWithOrders());
        }
    }
}

