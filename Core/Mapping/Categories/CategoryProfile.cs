
namespace Core.Mapping.Categories
{
    public partial class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            GetCategoryListMapping();
            GetCategoryByIdMapping();
            AddCategoryCommandMapping();
            EditCategoryCommandMapping();
        }
    }
}
