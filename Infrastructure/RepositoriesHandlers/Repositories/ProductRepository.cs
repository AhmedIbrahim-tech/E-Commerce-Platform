namespace Infrastructure.RepositoriesHandlers.Repositories;

public interface IProductRepository : IGenericRepositoryAsync<Product>
{
    Task<Dictionary<Guid, string?>> GetProductsByIdsAsync(List<Guid> productIds);
}

internal class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
{
    private readonly DbSet<Product> _products;

    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _products = dbContext.Set<Product>();
    }

    public async Task<Dictionary<Guid, string?>> GetProductsByIdsAsync(List<Guid> productIds)
    {
        var products = await _products.Where(p => productIds.Contains(p.Id))
                                        .ToDictionaryAsync(p => p.Id, p => p.Name);
        return products;
    }
}
