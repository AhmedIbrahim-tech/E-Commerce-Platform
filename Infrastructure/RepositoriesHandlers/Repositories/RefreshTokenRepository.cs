namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IRefreshTokenRepository : IGenericRepositoryAsync<RefreshToken>
{
}

public class RefreshTokenRepository : GenericRepositoryAsync<RefreshToken>, IRefreshTokenRepository
{
    #region Fields
    private readonly DbSet<RefreshToken> _refreshTokens;
    #endregion

    #region Constructors
    public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _refreshTokens = dbContext.Set<RefreshToken>();
    }
    #endregion

    #region Handle Functions

    #endregion
}
