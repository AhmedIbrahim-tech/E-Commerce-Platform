using Domain.Entities.Promotions;

namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IGiftCardRepository : IGenericRepositoryAsync<GiftCard>
{
}

public class GiftCardRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<GiftCard>(dbcontext), IGiftCardRepository
{
    private readonly DbSet<GiftCard> _giftCards = dbcontext.Set<GiftCard>();
}
