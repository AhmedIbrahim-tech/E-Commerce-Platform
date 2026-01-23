namespace Application.Features.Tags.Commands.EditTag;

public class EditTagValidator : AbstractValidator<EditTagCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditTagValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(t => t.Id)
            .NotEmpty().WithMessage("Tag ID is required")
            .NotNull().WithMessage("Tag ID is required");

        RuleFor(t => t.Name)
            .NotEmpty().WithMessage("Tag name is required")
            .NotNull().WithMessage("Tag name is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(t => t.Name)
            .MustAsync(async (model, name, cancellation) =>
                !await _unitOfWork.Tags.GetTableNoTracking().AnyAsync(x => x.Name.Equals(name) && x.Id != model.Id, cancellation))
            .WithMessage("Tag with this name already exists");
    }
}

