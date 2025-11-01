using Core.Features.Products.Commands.Models;

namespace Core.Features.Products.Commands.Handlers
{
    public class ProductCommandHandler : ApiResponseHandler,
        IRequestHandler<AddProductCommand, ApiResponse<string>>,
        IRequestHandler<EditProductCommand, ApiResponse<string>>,
        IRequestHandler<DeleteProductCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public ProductCommandHandler(IProductService productService, IMapper mapper) : base()
        {
            _productService = productService;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var productMapper = _mapper.Map<Product>(request);
            var result = await _productService.AddProductAsync(productMapper, request.ImageURL!);
            return result switch
            {
                "NoImage" => BadRequest<string>(SharedResourcesKeys.NoImage),
                "FailedInAdd" => BadRequest<string>(SharedResourcesKeys.CreateFailed),
                "FailedToUploadImage" => BadRequest<string>(SharedResourcesKeys.FailedToUploadImage),
                _ => Created("")
            };
        }

        public async Task<ApiResponse<string>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.Id);
            if (product == null) return NotFound<string>(SharedResourcesKeys.ProductNotFound);
            var productMapper = _mapper.Map(request, product);
            var result = await _productService.EditProductAsync(productMapper);
            if (result == "Success") return Edit("");
            else return BadRequest<string>(SharedResourcesKeys.UpdateFailed);
        }

        public async Task<ApiResponse<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.ProductId);
            if (product == null) return NotFound<string>(SharedResourcesKeys.ProductNotFound);
            var result = await _productService.DeleteProductAsync(product);
            if (result == "Success") return Deleted<string>();
            else return BadRequest<string>(SharedResourcesKeys.DeleteFailed);
        }
        #endregion
    }
}
