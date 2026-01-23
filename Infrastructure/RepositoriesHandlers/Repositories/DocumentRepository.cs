using Domain.Entities.Documents;

namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IDocumentRepository : IGenericRepositoryAsync<Document>
{
}

public class DocumentRepository(ApplicationDbContext dbcontext) : GenericRepositoryAsync<Document>(dbcontext), IDocumentRepository
{
    private readonly DbSet<Document> _documents = dbcontext.Set<Document>();
}

