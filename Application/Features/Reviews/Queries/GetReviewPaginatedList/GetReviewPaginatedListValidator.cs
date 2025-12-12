namespace Application.Features.Reviews.Queries.GetReviewPaginatedList;

public class GetReviewPaginatedListValidator : AbstractValidator<GetReviewPaginatedListQuery>
{
    public GetReviewPaginatedListValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

