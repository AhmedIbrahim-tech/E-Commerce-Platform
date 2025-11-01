using Core.Features.Products.Queries.Responses;

namespace Core.Mapping.Products
{
    public partial class ProductProfile
    {
        public void GetProductByIdMapping()
        {
            CreateMap<Product, GetSingleProductResponse>()
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.Reviews, opt => opt.Ignore());
        }
    }
}
