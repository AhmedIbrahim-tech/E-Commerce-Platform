using System.Text.Json;

namespace Core.Features.Payments.Commands.Models
{
    public record ServerCallbackCommand(JsonElement Payload, string Hmac) : IRequest<ApiResponse<string>>;
}
