namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface ISubCategoryRepository : IGenericRepositoryAsync<SubCategory>
{
}

public class SubCategoryRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<SubCategory>(dbcontext), ISubCategoryRepository
{
    private readonly DbSet<SubCategory> _subCategories = dbcontext.Set<SubCategory>();
}
