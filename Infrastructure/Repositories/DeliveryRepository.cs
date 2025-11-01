
namespace Infrastructure.Repositories
{
    public class DeliveryRepository : GenericRepositoryAsync<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
