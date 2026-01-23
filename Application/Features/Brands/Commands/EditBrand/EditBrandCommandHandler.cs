using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Brands.Commands.EditBrand;

public class EditBrandCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditBrandCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await unitOfWork.Brands.GetTableNoTracking()
            .Where(b => b.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (brand == null) return new ApiResponse<string>(BrandErrors.BrandNotFound());

        brand.Name = request.Name;
        brand.Description = request.Description;
        brand.ImageUrl = request.ImageUrl;
        brand.IsActive = request.IsActive;

        try
        {
            await unitOfWork.Brands.UpdateAsync(brand, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(BrandErrors.DuplicatedBrandName());
        }
    }
}
