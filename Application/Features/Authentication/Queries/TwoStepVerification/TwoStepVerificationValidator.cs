namespace Application.Features.Authentication.TwoStepVerification;

public class TwoStepVerificationValidator : AbstractValidator<TwoStepVerificationQuery>
{
    public TwoStepVerificationValidator()
    {
        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Code is required")
            .NotNull().WithMessage("Code is required");
    }
}
