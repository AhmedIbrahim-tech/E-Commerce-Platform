using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteBrandCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await unitOfWork.Brands.GetTableNoTracking()
            .Where(b => b.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (brand == null) return new ApiResponse<string>(BrandErrors.BrandNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Brands.DeleteAsync(brand, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<string>(BrandErrors.CannotDeleteBrandWithProducts());
        }
    }
}
