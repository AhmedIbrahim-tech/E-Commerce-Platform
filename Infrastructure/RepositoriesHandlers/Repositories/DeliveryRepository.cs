namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IDeliveryRepository : IGenericRepositoryAsync<Delivery>
{
}

public class DeliveryRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Delivery>(dbContext), IDeliveryRepository
{
}
