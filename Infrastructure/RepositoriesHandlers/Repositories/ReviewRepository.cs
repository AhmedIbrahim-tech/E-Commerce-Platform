namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IReviewRepository : IGenericRepositoryAsync<Review>
{
}

public class ReviewRepository : GenericRepositoryAsync<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
