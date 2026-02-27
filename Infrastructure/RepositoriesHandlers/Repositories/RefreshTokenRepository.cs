namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IRefreshTokenRepository : IGenericRepositoryAsync<RefreshToken>
{
}

public class RefreshTokenRepository(ApplicationDbContext dbContext) 
    : GenericRepositoryAsync<RefreshToken>(dbContext), IRefreshTokenRepository
{
}
