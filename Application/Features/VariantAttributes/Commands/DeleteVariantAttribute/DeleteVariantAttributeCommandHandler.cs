using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.VariantAttributes.Commands.DeleteVariantAttribute;

public class DeleteVariantAttributeCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteVariantAttributeCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteVariantAttributeCommand request, CancellationToken cancellationToken)
    {
        var variantAttribute = await unitOfWork.VariantAttributes.GetTableNoTracking()
            .Where(va => va.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (variantAttribute == null) return new ApiResponse<string>(VariantAttributeErrors.VariantAttributeNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.VariantAttributes.DeleteAsync(variantAttribute, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>("Cannot delete variant attribute");
        }
    }
}
