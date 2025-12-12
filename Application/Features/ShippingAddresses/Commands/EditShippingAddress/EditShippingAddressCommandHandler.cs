namespace Application.Features.ShippingAddresses.Commands.EditShippingAddress;

public class EditShippingAddressCommandHandler : ApiResponseHandler,
    IRequestHandler<EditShippingAddressCommand, ApiResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditShippingAddressCommandHandler(IUnitOfWork unitOfWork) : base()
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<string>> Handle(EditShippingAddressCommand request, CancellationToken cancellationToken)
    {
        var shippingAddress = await _unitOfWork.ShippingAddresses.GetTableAsTracking()
            .Where(c => c.Id.Equals(request.Id))
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(cancellationToken);

        if (shippingAddress == null) return new ApiResponse<string>(ShippingAddressErrors.ShippingAddressNotFound());

        shippingAddress.FirstName = request.FirstName;
        shippingAddress.LastName = request.LastName;
        shippingAddress.Street = request.Street;
        shippingAddress.City = request.City;
        shippingAddress.State = request.State;

        try
        {
            await _unitOfWork.ShippingAddresses.UpdateAsync(shippingAddress);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ShippingAddressErrors.InvalidAddress());
        }
    }
}

