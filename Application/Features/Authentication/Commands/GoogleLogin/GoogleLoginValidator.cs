namespace Application.Features.Authentication.Commands.GoogleLogin;

internal class GoogleLoginValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.IdToken)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

