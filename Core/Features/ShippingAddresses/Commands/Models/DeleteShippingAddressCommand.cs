
namespace Core.Features.ShippingAddresses.Commands.Models
{
    public record DeleteShippingAddressCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
