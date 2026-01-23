namespace Application.Features.ApplicationUser.Commands.EditMyProfile;

public class EditMyProfileCommandValidator : AbstractValidator<EditMyProfileCommand>
{
    public EditMyProfileCommandValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(25).WithMessage("Maximum length is 25 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}

