
namespace Infrastructure.Repositories
{
    public class OrderItemRepository : GenericRepositoryAsync<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
