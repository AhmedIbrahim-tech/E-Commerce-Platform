
namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly IdentityTablesSettings? _identityTablesSettings;

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options, 
            IEncryptionProvider encryptionProvider,
            IOptions<IdentityTablesSettings>? identityTablesSettings = null) : base(options)
        {
            _encryptionProvider = encryptionProvider;
            _identityTablesSettings = identityTablesSettings?.Value;
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure custom Identity table names if settings are provided
            if (_identityTablesSettings != null)
            {
                modelBuilder.Entity<User>().ToTable(_identityTablesSettings.Users);
                modelBuilder.Entity<Role>().ToTable(_identityTablesSettings.Roles);
                modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable(_identityTablesSettings.UserClaims);
                modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable(_identityTablesSettings.UserRoles);
                modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable(_identityTablesSettings.UserLogins);
                modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable(_identityTablesSettings.RoleClaims);
                modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable(_identityTablesSettings.UserTokens);
            }

            /// Execute all Configurations that implement from IEntityTypeConfiguration<>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            /// Configure encryption for sensitive data
            modelBuilder.UseEncryption(_encryptionProvider);
        }
    }
}
