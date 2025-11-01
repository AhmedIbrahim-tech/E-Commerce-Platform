
namespace Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepositoryAsync<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
