namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface ICategoryRepository : IGenericRepositoryAsync<Category>
{
}

public class CategoryRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Category>(dbcontext), ICategoryRepository
{
    private readonly DbSet<Category> _categories = dbcontext.Set<Category>();
}
