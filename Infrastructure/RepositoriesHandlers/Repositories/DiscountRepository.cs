using Domain.Entities.Promotions;

namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IDiscountRepository : IGenericRepositoryAsync<Discount>
{
}

public class DiscountRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Discount>(dbcontext), IDiscountRepository
{
    private readonly DbSet<Discount> _discounts = dbcontext.Set<Discount>();
}

public interface IDiscountPlanRepository : IGenericRepositoryAsync<DiscountPlan>
{
}

public class DiscountPlanRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<DiscountPlan>(dbcontext), IDiscountPlanRepository
{
    private readonly DbSet<DiscountPlan> _discountPlans = dbcontext.Set<DiscountPlan>();
}
