namespace Infrastructure.RepositoriesHandlers.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    IOrderItemRepository OrderItems { get; }
    IReviewRepository Reviews { get; }
    IShippingAddressRepository ShippingAddresses { get; }
    IPaymentRepository Payments { get; }
    IDeliveryRepository Deliveries { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}


public class UnitOfWork(ApplicationDbContext Context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    private ICategoryRepository? _categories;
    private IProductRepository? _products;
    private IOrderRepository? _orders;
    private IOrderItemRepository? _orderItems;
    private IReviewRepository? _reviews;
    private IShippingAddressRepository? _shippingAddresses;
    private IPaymentRepository? _payments;
    private IDeliveryRepository? _deliveries;
    private IRefreshTokenRepository? _refreshTokens;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await Context.Database.BeginTransactionAsync(cancellationToken);
        return _transaction;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }


    public ICategoryRepository Categories => _categories ??= new CategoryRepository(Context);

    public IProductRepository Products => _products ??= new ProductRepository(Context);

    public IOrderRepository Orders => _orders ??= new OrderRepository(Context);

    public IOrderItemRepository OrderItems => _orderItems ??= new OrderItemRepository(Context);

    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(Context);

    public IShippingAddressRepository ShippingAddresses => _shippingAddresses ??= new ShippingAddressRepository(Context);

    public IPaymentRepository Payments => _payments ??= new PaymentRepository(Context);

    public IDeliveryRepository Deliveries => _deliveries ??= new DeliveryRepository(Context);

    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(Context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }


    public void Dispose()
    {
        _transaction?.Dispose();
    }
}

