
namespace Core.Mapping.Products
{
    public partial class ProductProfile : Profile
    {
        public ProductProfile()
        {
            GetProductByIdMapping();
            AddProductCommandMapping();
            EditProductCommandMapping();
        }
    }
}
