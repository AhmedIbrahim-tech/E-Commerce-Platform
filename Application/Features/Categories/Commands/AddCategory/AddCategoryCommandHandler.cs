namespace Application.Features.Categories.Commands.AddCategory;

public class AddCategoryCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddCategoryCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            await unitOfWork.Categories.AddAsync(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CategoryErrors.DuplicatedCategoryName());
        }
    }
}

