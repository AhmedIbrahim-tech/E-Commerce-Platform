using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<DeleteProductCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetTableAsTracking()
            .Where(c => c.Id.Equals(request.ProductId))
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null) return NotFound<string>("Product not found");

        if (product.IsDeleted) return Success("Product is already deleted");

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var currentUserId = currentUserService.GetUserId();
            product.MarkDeleted(currentUserId);
            
            await unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>("Product deleted successfully");
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>("Cannot delete product. It may have associated orders or other dependencies.");
        }
    }
}

