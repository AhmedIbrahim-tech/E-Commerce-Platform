using Core.Features.Products.Commands.Models;

namespace Core.Mapping.Products
{
    public partial class ProductProfile
    {
        public void AddProductCommandMapping()
        {
            CreateMap<AddProductCommand, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow.ToLocalTime()))
                .ForMember(dest => dest.ImageURL, opt => opt.Ignore());
        }
    }
}
