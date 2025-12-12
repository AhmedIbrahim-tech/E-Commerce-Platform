using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.AddProduct;

public class AddProductCommandHandler(
    IUnitOfWork unitOfWork,
    IFileService fileService,
    IHttpContextAccessor httpContextAccessor) : ApiResponseHandler(),
    IRequestHandler<AddProductCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext!.Request;
        var baseUrl = context.Scheme + "://" + context.Host;
        var imageUrl = await fileService.UploadImageAsync("Products", request.ImageURL!);
        
        switch (imageUrl)
        {
            case "FailedToUploadImage": return new ApiResponse<string>(ProductErrors.FailedToUploadImage());
            case "NoImage": return new ApiResponse<string>(ProductErrors.ImageRequired());
        }

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            ImageURL = baseUrl + imageUrl
        };

        try
        {
            await unitOfWork.Products.AddAsync(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ProductErrors.InvalidPrice());
        }
    }
}

