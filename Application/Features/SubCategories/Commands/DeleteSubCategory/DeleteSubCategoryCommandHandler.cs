using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.SubCategories.Commands.DeleteSubCategory;

public class DeleteSubCategoryCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteSubCategoryCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await unitOfWork.SubCategories.GetTableNoTracking()
            .Where(sc => sc.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (subCategory == null) return new ApiResponse<string>(SubCategoryErrors.SubCategoryNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.SubCategories.DeleteAsync(subCategory, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<string>(SubCategoryErrors.CannotDeleteSubCategoryWithProducts());
        }
    }
}
