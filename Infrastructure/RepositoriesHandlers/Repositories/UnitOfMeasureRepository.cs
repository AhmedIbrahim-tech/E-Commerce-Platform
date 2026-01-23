namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IUnitOfMeasureRepository : IGenericRepositoryAsync<UnitOfMeasure>
{
}

public class UnitOfMeasureRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<UnitOfMeasure>(dbcontext), IUnitOfMeasureRepository
{
    private readonly DbSet<UnitOfMeasure> _unitOfMeasures = dbcontext.Set<UnitOfMeasure>();
}
