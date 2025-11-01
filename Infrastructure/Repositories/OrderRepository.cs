
namespace Infrastructure.Repositories
{
    public class OrderRepository : GenericRepositoryAsync<Order>, IOrderRepository
    {
        #region Fields
        private readonly DbSet<Order> _orders;
        #endregion

        #region Constructors
        public OrderRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _orders = dbContext.Set<Order>();
        }
        #endregion

        #region Handle Functions

        #endregion
    }
}
