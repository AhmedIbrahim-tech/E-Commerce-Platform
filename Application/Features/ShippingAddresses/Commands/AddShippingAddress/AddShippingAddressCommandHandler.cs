namespace Application.Features.ShippingAddresses.Commands.AddShippingAddress;

public class AddShippingAddressCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<AddShippingAddressCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddShippingAddressCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = currentUserService.GetUserId();
            var shippingAddress = new ShippingAddress
            {
                FullName = request.FullName,
                Street = request.Street,
                City = request.City,
                State = request.State,
                CustomerId = currentUserId
            };

            await unitOfWork.ShippingAddresses.AddAsync(shippingAddress, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ShippingAddressErrors.InvalidAddress());
        }
    }
}
