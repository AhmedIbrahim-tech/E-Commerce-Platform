using Application.Common.Bases;
using System.Text.Json;

namespace Application.Features.Payments.Commands.ServerCallback;

public record ServerCallbackCommand(JsonElement Payload, string Hmac) : IRequest<ApiResponse<string>>;

