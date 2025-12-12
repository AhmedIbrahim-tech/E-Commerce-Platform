using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteProductCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.ProductId))
            .Include(c => c.Category)
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null) return new ApiResponse<string>(ProductErrors.ProductNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Products.DeleteAsync(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<string>(ProductErrors.CannotDeleteProductWithOrders());
        }
    }
}

