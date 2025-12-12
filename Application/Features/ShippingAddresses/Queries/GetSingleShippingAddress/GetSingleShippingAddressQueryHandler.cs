namespace Application.Features.ShippingAddresses.Queries.GetSingleShippingAddress;

public class GetSingleShippingAddressQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetSingleShippingAddressQuery, ApiResponse<GetSingleShippingAddressResponse>>
{
    public async Task<ApiResponse<GetSingleShippingAddressResponse>> Handle(GetSingleShippingAddressQuery request, CancellationToken cancellationToken)
    {
        var shippingAddress = await unitOfWork.ShippingAddresses.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.Id))
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(cancellationToken);

        if (shippingAddress is null) return new ApiResponse<GetSingleShippingAddressResponse>(ShippingAddressErrors.ShippingAddressNotFound());

        var shippingAddressResponse = new GetSingleShippingAddressResponse(
            shippingAddress.Id,
            shippingAddress.FirstName,
            shippingAddress.LastName,
            shippingAddress.Street,
            shippingAddress.City,
            shippingAddress.State
        );

        return Success(shippingAddressResponse);
    }
}

