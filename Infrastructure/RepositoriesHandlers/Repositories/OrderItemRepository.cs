namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IOrderItemRepository : IGenericRepositoryAsync<OrderItem>
{
}

public class OrderItemRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<OrderItem>(dbContext), IOrderItemRepository
{
}
