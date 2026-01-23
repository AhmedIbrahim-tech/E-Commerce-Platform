namespace Application.Features.Brands.Commands.AddBrand;

public class AddBrandCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddBrandCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddBrandCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var brand = new Brand
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive
            };

            await unitOfWork.Brands.AddAsync(brand, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(BrandErrors.DuplicatedBrandName());
        }
    }
}
