
namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepositoryAsync<Category>, ICategoryRepository
    {
        private readonly DbSet<Category> _categories;

        public CategoryRepository(ApplicationContext dbcontext) : base(dbcontext)
        {
            _categories = dbcontext.Set<Category>();
        }
    }
}
