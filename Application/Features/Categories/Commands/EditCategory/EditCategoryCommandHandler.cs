using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Categories.Commands.EditCategory;

public class EditCategoryCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditCategoryCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Categories.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.Id))
            .FirstOrDefaultAsync();

        if (category == null) return new ApiResponse<string>(CategoryErrors.CategoryNotFound());

        category.Name = request.Name;
        category.Description = request.Description;

        try
        {
            await unitOfWork.Categories.UpdateAsync(category);
            await unitOfWork.SaveChangesAsync();
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CategoryErrors.DuplicatedCategoryName());
        }
    }
}

