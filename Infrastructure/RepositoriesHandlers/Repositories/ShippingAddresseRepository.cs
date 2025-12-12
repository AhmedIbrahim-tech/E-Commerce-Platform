namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IShippingAddressRepository : IGenericRepositoryAsync<ShippingAddress>
{
}

public class ShippingAddressRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<ShippingAddress>(dbContext), IShippingAddressRepository
{
}
