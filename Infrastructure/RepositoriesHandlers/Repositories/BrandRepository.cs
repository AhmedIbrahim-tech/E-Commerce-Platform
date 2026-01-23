namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IBrandRepository : IGenericRepositoryAsync<Brand>
{
}

public class BrandRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Brand>(dbcontext), IBrandRepository
{
    private readonly DbSet<Brand> _brands = dbcontext.Set<Brand>();
}
