using Domain.Entities.Accounts;

namespace Infrastructure.RepositoriesHandlers.Repositories.Accounts;

public interface IAccountRepository : IGenericRepositoryAsync<Account>
{
}

public class AccountRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Account>(dbcontext), IAccountRepository
{
    private readonly DbSet<Account> _accounts = dbcontext.Set<Account>();
}
