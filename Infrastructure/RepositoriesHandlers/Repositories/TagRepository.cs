namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface ITagRepository : IGenericRepositoryAsync<Tag>
{
}

public class TagRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Tag>(dbcontext), ITagRepository
{
    private readonly DbSet<Tag> _tags = dbcontext.Set<Tag>();
}

