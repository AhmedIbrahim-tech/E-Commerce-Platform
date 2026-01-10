namespace Application.Features.Authentication.Commands.SendResetPassword;

public class SendResetPasswordValidator : AbstractValidator<SendResetPasswordCommand>
{
    public SendResetPasswordValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

