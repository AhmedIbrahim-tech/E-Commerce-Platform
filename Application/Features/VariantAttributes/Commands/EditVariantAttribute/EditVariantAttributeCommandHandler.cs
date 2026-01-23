using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.VariantAttributes.Commands.EditVariantAttribute;

public class EditVariantAttributeCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditVariantAttributeCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditVariantAttributeCommand request, CancellationToken cancellationToken)
    {
        var variantAttribute = await unitOfWork.VariantAttributes.GetTableNoTracking()
            .Where(va => va.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (variantAttribute == null) return new ApiResponse<string>(VariantAttributeErrors.VariantAttributeNotFound());

        variantAttribute.Name = request.Name;
        variantAttribute.Description = request.Description;
        variantAttribute.IsActive = request.IsActive;

        try
        {
            await unitOfWork.VariantAttributes.UpdateAsync(variantAttribute, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(VariantAttributeErrors.DuplicatedVariantAttributeName());
        }
    }
}
