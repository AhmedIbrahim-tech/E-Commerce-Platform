namespace Infrastructure.RepositoriesHandlers.Bases;

public interface IGenericRepositoryAsync<T> where T : class
{
    // Query Operations
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T?>> GetAllAsync(CancellationToken cancellationToken = default);
    IQueryable<T> GetTableNoTracking();
    IQueryable<T> GetTableAsTracking();

    // Create Operations
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);

    // Update Operations
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);

    // Delete Operations
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);

    // Transaction Operations
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    // Utility Operations
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void AttachEntity<TEntity>(TEntity entity) where TEntity : class;
}

public class GenericRepositoryAsync<T>(ApplicationDbContext dbContext) : IGenericRepositoryAsync<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    #region Query Operations

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T?>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
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

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return;

        await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
    }

    #endregion

    #region Update Operations

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<T>().Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return;

        _dbContext.Set<T>().UpdateRange(entities);
        await Task.CompletedTask;
    }

    #endregion

    #region Delete Operations

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return;

        _dbContext.Set<T>().RemoveRange(entities);
        await Task.CompletedTask;
    }

    #endregion

    #region Transaction Operations

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    #endregion

    #region Utility Operations

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void AttachEntity<TEntity>(TEntity entity) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<TEntity>().Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Unchanged;
    }

    #endregion
}
