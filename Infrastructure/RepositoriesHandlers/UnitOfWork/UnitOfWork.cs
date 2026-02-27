using Infrastructure.RepositoriesHandlers.Repositories.Accounts;
using Infrastructure.RepositoriesHandlers.Repositories.Users;

namespace Infrastructure.RepositoriesHandlers.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ApplicationDbContext Context { get; }
    ICategoryRepository Categories { get; }
    ISubCategoryRepository SubCategories { get; }
    IBrandRepository Brands { get; }
    IUnitOfMeasureRepository UnitOfMeasures { get; }
    IWarrantyRepository Warranties { get; }
    IVariantAttributeRepository VariantAttributes { get; }
    ITagRepository Tags { get; }
    ICouponRepository Coupons { get; }
    IGiftCardRepository GiftCards { get; }
    IDiscountRepository Discounts { get; }
    IDiscountPlanRepository DiscountPlans { get; }
    IAccountRepository Accounts { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    IOrderItemRepository OrderItems { get; }
    IReviewRepository Reviews { get; }
    IShippingAddressRepository ShippingAddresses { get; }
    IPaymentRepository Payments { get; }
    IDeliveryRepository Deliveries { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    INotificationRepository Notifications { get; }
    IDocumentRepository Documents { get; }
    IAdminRepository Admins { get; }
    ICustomerRepository Customers { get; }
    IVendorRepository Vendors { get; }
    IAuditLogRepository AuditLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}


public class UnitOfWork(ApplicationDbContext Context) : IUnitOfWork
{
    public ApplicationDbContext Context { get; } = Context;
    private IDbContextTransaction? _transaction;

    private ICategoryRepository? _categories;
    private ISubCategoryRepository? _subCategories;
    private IBrandRepository? _brands;
    private IUnitOfMeasureRepository? _unitOfMeasures;
    private IWarrantyRepository? _warranties;
    private IVariantAttributeRepository? _variantAttributes;
    private ITagRepository? _tags;
    private ICouponRepository? _coupons;
    private IGiftCardRepository? _giftCards;
    private IDiscountRepository? _discounts;
    private IDiscountPlanRepository? _discountPlans;
    private IAccountRepository? _accounts;
    private IProductRepository? _products;
    private IOrderRepository? _orders;
    private IOrderItemRepository? _orderItems;
    private IReviewRepository? _reviews;
    private IShippingAddressRepository? _shippingAddresses;
    private IPaymentRepository? _payments;
    private IDeliveryRepository? _deliveries;
    private IRefreshTokenRepository? _refreshTokens;
    private INotificationRepository? _notifications;
    private IDocumentRepository? _documents;
    private IAdminRepository? _admins;
    private ICustomerRepository? _customers;
    private IVendorRepository? _vendors;
    private IAuditLogRepository? _auditLogs;

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

    public ISubCategoryRepository SubCategories => _subCategories ??= new SubCategoryRepository(Context);

    public IBrandRepository Brands => _brands ??= new BrandRepository(Context);

    public IUnitOfMeasureRepository UnitOfMeasures => _unitOfMeasures ??= new UnitOfMeasureRepository(Context);

    public IWarrantyRepository Warranties => _warranties ??= new WarrantyRepository(Context);

    public IVariantAttributeRepository VariantAttributes => _variantAttributes ??= new VariantAttributeRepository(Context);

    public ITagRepository Tags => _tags ??= new TagRepository(Context);

    public ICouponRepository Coupons => _coupons ??= new CouponRepository(Context);

    public IGiftCardRepository GiftCards => _giftCards ??= new GiftCardRepository(Context);

    public IDiscountRepository Discounts => _discounts ??= new DiscountRepository(Context);

    public IDiscountPlanRepository DiscountPlans => _discountPlans ??= new DiscountPlanRepository(Context);

    public IAccountRepository Accounts => _accounts ??= new AccountRepository(Context);

    public IProductRepository Products => _products ??= new ProductRepository(Context);

    public IOrderRepository Orders => _orders ??= new OrderRepository(Context);

    public IOrderItemRepository OrderItems => _orderItems ??= new OrderItemRepository(Context);

    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(Context);

    public IShippingAddressRepository ShippingAddresses => _shippingAddresses ??= new ShippingAddressRepository(Context);

    public IPaymentRepository Payments => _payments ??= new PaymentRepository(Context);

    public IDeliveryRepository Deliveries => _deliveries ??= new DeliveryRepository(Context);

    public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(Context);

    public INotificationRepository Notifications => _notifications ??= new NotificationRepository(Context);

    public IDocumentRepository Documents => _documents ??= new DocumentRepository(Context);

    public IAdminRepository Admins => _admins ??= new AdminRepository(Context);

    public ICustomerRepository Customers => _customers ??= new CustomerRepository(Context);

    public IVendorRepository Vendors => _vendors ??= new VendorRepository(Context);

    public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(Context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }


    public void Dispose()
    {
        _transaction?.Dispose();
    }
}

