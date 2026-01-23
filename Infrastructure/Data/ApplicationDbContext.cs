using System.Linq.Expressions;
using Domain.Entities.Accounts;
using Domain.Entities.AuditLogs;
using Domain.Entities.Catalog;
using Domain.Entities.Documents;
using Domain.Entities.Notifications;
using Domain.Entities.Orders;
using Domain.Entities.Promotions;
using Domain.Entities.Reviews;
using Domain.Entities.Shipping;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IEncryptionProvider encryptionProvider,
    IHttpContextAccessor httpContextAccessor) : IdentityDbContext<AppUser, AppRole, Guid,
    IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<RefreshToken> UserRefreshTokens { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    public DbSet<Warranty> Warranties { get; set; }
    public DbSet<VariantAttribute> VariantAttributes { get; set; }
    public DbSet<VariantAttributeValue> VariantAttributeValues { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<GiftCard> GiftCards { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<DiscountPlan> DiscountPlans { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ShippingAddress> ShippingAddresses { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>().ToTable("Users");
        modelBuilder.Entity<AppRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ApplyGlobalQueryFilterForSoftDelete(modelBuilder);

        modelBuilder.UseEncryption(encryptionProvider);
    }

    private void ApplyGlobalQueryFilterForSoftDelete(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }

    public override int SaveChanges()
    {
        HandleAuditableEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleAuditableEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void HandleAuditableEntities()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

        var currentUserId = GetCurrentUserId();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                var createdTimeProperty = entry.Property(nameof(IAuditable.CreatedTime));
                if (createdTimeProperty.CurrentValue == null || 
                    (createdTimeProperty.CurrentValue is DateTimeOffset dt && dt == default))
                {
                    createdTimeProperty.CurrentValue = DateTimeOffset.UtcNow;
                }

                var createdByProperty = entry.Property(nameof(IAuditable.CreatedBy));
                if (createdByProperty.CurrentValue == null || 
                    (createdByProperty.CurrentValue is Guid guid && guid == default))
                {
                    createdByProperty.CurrentValue = currentUserId;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(IAuditable.ModifiedTime)).CurrentValue = DateTimeOffset.UtcNow;
                entry.Property(nameof(IAuditable.ModifiedBy)).CurrentValue = currentUserId;
            }
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            ?.FirstOrDefault(c => c.Type == "Id");

        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return Guid.Empty;
    }
}

