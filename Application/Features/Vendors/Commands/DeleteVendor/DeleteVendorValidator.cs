using FluentValidation;

namespace Application.Features.Vendors.Commands.DeleteVendor;

public class DeleteVendorValidator : AbstractValidator<DeleteVendorCommand>
{
    public DeleteVendorValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Vendor ID is required");
    }
}
