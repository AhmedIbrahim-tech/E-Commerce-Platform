using Core.Features.Products.Commands.Models;

namespace Core.Mapping.Products
{
    public partial class ProductProfile
    {
        public void EditProductCommandMapping()
        {
            CreateMap<EditProductCommand, Product>();
        }
    }
}
