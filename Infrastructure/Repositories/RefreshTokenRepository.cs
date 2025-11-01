
namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
    {
        #region Fields
        private readonly DbSet<RefreshToken> _refreshTokens;
        #endregion

        #region Constructors
        public RefreshTokenRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _refreshTokens = dbContext.Set<RefreshToken>();
        }
        #endregion

        #region Handle Functions

        #endregion
    }
}
