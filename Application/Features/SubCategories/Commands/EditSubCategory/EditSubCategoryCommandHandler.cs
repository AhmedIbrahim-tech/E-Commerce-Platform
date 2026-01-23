using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.SubCategories.Commands.EditSubCategory;

public class EditSubCategoryCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditSubCategoryCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await unitOfWork.SubCategories.GetTableNoTracking()
            .Where(sc => sc.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (subCategory == null) return new ApiResponse<string>(SubCategoryErrors.SubCategoryNotFound());

        var categoryExists = await unitOfWork.Categories.GetTableNoTracking()
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            return new ApiResponse<string>(SubCategoryErrors.InvalidCategory());

        subCategory.Name = request.Name;
        subCategory.Description = request.Description;
        subCategory.ImageUrl = request.ImageUrl;
        subCategory.Code = request.Code;
        subCategory.CategoryId = request.CategoryId;
        subCategory.IsActive = request.IsActive;

        try
        {
            await unitOfWork.SubCategories.UpdateAsync(subCategory, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(SubCategoryErrors.DuplicatedSubCategoryName());
        }
    }
}
