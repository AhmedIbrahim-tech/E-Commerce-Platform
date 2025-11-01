
namespace Infrastructure.Repositories
{
    public class ShippingAddressRepository : GenericRepositoryAsync<ShippingAddress>, IShippingAddressRepository
    {
        public ShippingAddressRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
