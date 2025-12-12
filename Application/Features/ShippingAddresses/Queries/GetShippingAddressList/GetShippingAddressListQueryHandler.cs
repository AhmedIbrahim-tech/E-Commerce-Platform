namespace Application.Features.ShippingAddresses.Queries.GetShippingAddressList;

public class GetShippingAddressListQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<GetShippingAddressListQuery, ApiResponse<List<GetShippingAddressListResponse>>>
{
    public async Task<ApiResponse<List<GetShippingAddressListResponse>>> Handle(GetShippingAddressListQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetUserId();
        var shippingAddressList = await unitOfWork.ShippingAddresses.GetTableNoTracking()
            .Where(c => c.CustomerId.Equals(currentUserId))
            .ToListAsync(cancellationToken);

        var shippingAddressListResponse = shippingAddressList
            .Select(address => new GetShippingAddressListResponse(
                address.Id,
                address.Street,
                address.City,
                address.State
            ))
            .ToList();

        return Success(shippingAddressListResponse);
    }
}

