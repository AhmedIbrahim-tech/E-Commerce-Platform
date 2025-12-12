namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
{
}

public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
{
    #region Fields
    private readonly DbSet<UserRefreshToken> _refreshTokens;
    #endregion

    #region Constructors
    public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _refreshTokens = dbContext.Set<UserRefreshToken>();
    }
    #endregion

    #region Handle Functions

    #endregion
}
