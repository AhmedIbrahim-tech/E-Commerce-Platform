using System.Text.Json;

namespace Application.Features.Payments.Commands.ServerCallback;

public class ServerCallbackValidator : AbstractValidator<ServerCallbackCommand>
{
    public ServerCallbackValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.Payload.ValueKind)
            .NotEqual(JsonValueKind.Undefined).WithMessage("Invalid payload")
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.Hmac)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

