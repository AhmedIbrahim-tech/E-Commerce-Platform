using Core.Features.Payments.Commands.Models;
using System.Text.Json;

namespace Core.Features.Payments.Commands.Validators
{
    public class ServerCallbackCommandValidator : AbstractValidator<ServerCallbackCommand>
    {
        #region Constructors
        public ServerCallbackCommandValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Payload.ValueKind)
                .NotEqual(JsonValueKind.Undefined).WithMessage(SharedResourcesKeys.InvalidPayload)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.Hmac)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}
