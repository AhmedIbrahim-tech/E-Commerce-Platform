namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IOrderRepository : IGenericRepositoryAsync<Order>
{
}

public class OrderRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Order>(dbContext), IOrderRepository
{
    #region Fields
    private readonly DbSet<Order> _orders = dbContext.Set<Order>();
    #endregion

    #region Handle Functions

    #endregion
}
