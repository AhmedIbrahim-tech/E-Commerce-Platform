namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IVariantAttributeRepository : IGenericRepositoryAsync<VariantAttribute>
{
}

public class VariantAttributeRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<VariantAttribute>(dbcontext), IVariantAttributeRepository
{
    private readonly DbSet<VariantAttribute> _variantAttributes = dbcontext.Set<VariantAttribute>();
}
