namespace Application.Features.Tags.Commands.AddTag;

public class AddTagValidator : AbstractValidator<AddTagCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddTagValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("Tag name is required")
            .NotNull().WithMessage("Tag name is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(t => t.Name)
            .MustAsync(async (name, cancellation) =>
                !await _unitOfWork.Tags.GetTableNoTracking().AnyAsync(x => x.Name.Equals(name), cancellation))
            .WithMessage("Tag with this name already exists");
    }
}

