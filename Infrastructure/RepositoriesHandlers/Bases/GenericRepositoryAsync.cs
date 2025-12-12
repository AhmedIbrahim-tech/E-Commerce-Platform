namespace Infrastructure.RepositoriesHandlers.Bases;

public interface IGenericRepositoryAsync<T> where T : class
{
    // Query Operations
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T?>> GetAllAsync();
    IQueryable<T> GetTableNoTracking();
    IQueryable<T> GetTableAsTracking();

    // Create Operations
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);

    // Update Operations
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(ICollection<T> entities);

    // Delete Operations
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(ICollection<T> entities);

    // Transaction Operations
    Task<IDbContextTransaction> BeginTransactionAsync();

    // Utility Operations
    Task SaveChangesAsync();
    void AttachEntity<TEntity>(TEntity entity) where TEntity : class;
}

public class GenericRepositoryAsync<T>(ApplicationDbContext dbContext) : IGenericRepositoryAsync<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    #region Query Operations

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public virtual async Task<IReadOnlyList<T?>> GetAllAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public IQueryable<T> GetTableNoTracking()
    {
        return _dbContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> GetTableAsTracking()
    {
        return _dbContext.Set<T>();
    }

    #endregion

    #region Create Operations

    public virtual async Task<T> AddAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public virtual async Task AddRangeAsync(ICollection<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return;

        await _dbContext.Set<T>().AddRangeAsync(entities);
    }

    #endregion

    #region Update Operations

    public virtual async Task UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<T>().Update(entity);
    }

    public virtual async Task UpdateRangeAsync(ICollection<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return;

        _dbContext.Set<T>().UpdateRange(entities);
    }

    #endregion

    #region Delete Operations

    public virtual async Task DeleteAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<T>().Remove(entity);
    }

    public virtual async Task DeleteRangeAsync(ICollection<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return;

        _dbContext.Set<T>().RemoveRange(entities);
    }

    #endregion

    #region Transaction Operations

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    #endregion

    #region Utility Operations

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void AttachEntity<TEntity>(TEntity entity) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<TEntity>().Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Unchanged;
    }

    #endregion
}
