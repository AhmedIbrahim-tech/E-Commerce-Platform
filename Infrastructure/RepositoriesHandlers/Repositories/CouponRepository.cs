using Domain.Entities.Promotions;

namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface ICouponRepository : IGenericRepositoryAsync<Coupon>
{
}

public class CouponRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Coupon>(dbcontext), ICouponRepository
{
    private readonly DbSet<Coupon> _coupons = dbcontext.Set<Coupon>();
}
