using Domain.Entities.Catalog;

namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IWarrantyRepository : IGenericRepositoryAsync<Warranty>
{
}

public class WarrantyRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Warranty>(dbcontext), IWarrantyRepository
{
    private readonly DbSet<Warranty> _warranties = dbcontext.Set<Warranty>();
}
