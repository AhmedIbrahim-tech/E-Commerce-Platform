using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.EditProduct;

public class EditProductCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditProductCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetTableAsTracking()
            .Where(c => c.Id.Equals(request.Id))
            .Include(c => c.Category)
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null) return new ApiResponse<string>(ProductErrors.ProductNotFound());

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.CategoryId = request.CategoryId;
        product.Category = null;

        try
        {
            await unitOfWork.Products.UpdateAsync(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ProductErrors.InvalidPrice());
        }
    }
}

