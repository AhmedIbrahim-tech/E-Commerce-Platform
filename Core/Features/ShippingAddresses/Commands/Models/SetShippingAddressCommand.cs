
namespace Core.Features.ShippingAddresses.Commands.Models
{
    public record SetShippingAddressCommand(Guid OrderId, Guid ShippingAddressId) : IRequest<ApiResponse<string>>;
}
