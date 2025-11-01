
namespace Infrastructure.Repositories.Contract
{
    public interface IProductRepository : IGenericRepositoryAsync<Product>
    {
        Task<Dictionary<Guid, string?>> GetProductsByIdsAsync(List<Guid> productIds);
    }
}
