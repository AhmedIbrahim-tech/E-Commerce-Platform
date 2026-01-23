namespace Application.Features.SubCategories.Commands.AddSubCategory;

public class AddSubCategoryCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddSubCategoryCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await unitOfWork.Categories.GetTableNoTracking()
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            return new ApiResponse<string>(SubCategoryErrors.InvalidCategory());

        try
        {
            var subCategory = new SubCategory
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Code = request.Code,
                CategoryId = request.CategoryId,
                IsActive = request.IsActive
            };

            await unitOfWork.SubCategories.AddAsync(subCategory, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(SubCategoryErrors.DuplicatedSubCategoryName());
        }
    }
}
