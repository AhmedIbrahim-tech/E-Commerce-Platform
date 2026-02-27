using Domain.Common.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoriesHandlers.Bases;

public interface IGenericRepositoryAsync<T> where T : class
{
    // Query Operations
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, bool includeDeleted, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
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

    // Utility Operations
    /// <summary>
    /// Saves all changes made in this context to the database.
    /// NOTE: Prefer using IUnitOfWork.SaveChangesAsync() for better transaction management.
    /// This method is available for special cases where direct repository access is needed (e.g., RefreshTokenRepository).
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void AttachEntity<TEntity>(TEntity entity) where TEntity : class;
    void AttachEntity<TEntity>(TEntity entity, EntityState state) where TEntity : class;
}

public class GenericRepositoryAsync<T>(ApplicationDbContext dbContext) : IGenericRepositoryAsync<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    #region Query Operations

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, includeDeleted: false, cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, bool includeDeleted, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (includeDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.FirstOrDefaultAsync(GetIdPredicate(id), cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>()
            .AnyAsync(GetIdPredicate(id), cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>()
            .CountAsync(cancellationToken);
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

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return Task.CompletedTask;

        _dbContext.Set<T>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    #endregion

    #region Delete Operations

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        if (entities.Count == 0)
            return Task.CompletedTask;

        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    #endregion

    #region Utility Operations

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void AttachEntity<TEntity>(TEntity entity) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);
        AttachEntity(entity, EntityState.Unchanged);
    }

    public void AttachEntity<TEntity>(TEntity entity, EntityState state) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);

        _dbContext.Set<TEntity>().Attach(entity);
        _dbContext.Entry(entity).State = state;
    }

    #endregion

    #region Helper Methods

    protected virtual System.Linq.Expressions.Expression<Func<T, bool>> GetIdPredicate(Guid id)
    {
        var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "e");
        var property = System.Linq.Expressions.Expression.Property(parameter, "Id");
        var constant = System.Linq.Expressions.Expression.Constant(id);
        var equality = System.Linq.Expressions.Expression.Equal(property, constant);
        return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equality, parameter);
    }

    #endregion
}
