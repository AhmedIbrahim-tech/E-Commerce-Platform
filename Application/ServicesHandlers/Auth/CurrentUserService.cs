using Infrastructure.Data.Identity;

namespace Application.ServicesHandlers.Auth;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    Guid GetUserId();
    Guid GetCartOwnerId();
    Task<User> GetUserAsync();
    Task<List<string>> GetCurrentUserRolesAsync();
    bool DeleteGuestIdCookie();
    string GetBaseUrl();
}

public class CurrentUserService : ICurrentUserService
{
    #region Fields
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    #endregion

    #region Helper
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    #endregion

    #region Constructors
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, ApplicationDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _dbContext = dbContext;
    }
    #endregion

    #region Functions
    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims?.SingleOrDefault(claim => claim.Type == nameof(UserClaimModel.Id))?.Value;
        if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("UnAuthenticated");
        return Guid.Parse(userId);
    }

    public Guid GetCartOwnerId()
    {
        if (IsAuthenticated) return GetUserId();

        var guestId = _httpContextAccessor.HttpContext?.Request.Cookies["GuestId"];
        if (string.IsNullOrEmpty(guestId) || !Guid.TryParse(guestId, out var parsedGuestId))
        {
            parsedGuestId = Guid.NewGuid();
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("GuestId", parsedGuestId.ToString(), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                IsEssential = true
            });
        }

        return parsedGuestId;
    }

    public async Task<User> GetUserAsync()
    {
        Guid userId = GetUserId();
        
        // Get Identity user
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser == null) throw new UnauthorizedAccessException();
        
        // Get domain entity (try Customer, Vendor, Admin)
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.AppUserId == appUser.Id);
        if (customer != null)
        {
            return new User
            {
                Id = customer.Id,
                AppUserId = customer.AppUserId,
                FullName = customer.FullName,
                Gender = customer.Gender
            };
        }
        
        var vendor = await _dbContext.Vendors.FirstOrDefaultAsync(v => v.AppUserId == appUser.Id);
        if (vendor != null)
        {
            return new User
            {
                Id = vendor.Id,
                AppUserId = vendor.AppUserId,
                FullName = vendor.FullName,
                Gender = vendor.Gender
            };
        }
        
        var admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.AppUserId == appUser.Id);
        if (admin != null)
        {
            return new User
            {
                Id = admin.Id,
                AppUserId = admin.AppUserId,
                FullName = admin.FullName,
                Gender = admin.Gender
            };
        }
        
        // If no domain entity found, return basic User with data from AppUser
        return new User
        {
            Id = appUser.Id,
            AppUserId = appUser.Id,
            FullName = appUser.DisplayName,
            Gender = null
        };
    }

    public async Task<List<string>> GetCurrentUserRolesAsync()
    {
        Guid userId = GetUserId();
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser == null) throw new UnauthorizedAccessException();
        
        var roles = await _userManager.GetRolesAsync(appUser);
        return roles.ToList();
    }

    public bool DeleteGuestIdCookie()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                if (httpContext.Request.Cookies.ContainsKey("GuestId"))
                {
                    httpContext.Response.Cookies.Delete("GuestId");
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GetBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return string.Empty;

        string schema = request.Scheme.EndsWith("s") ? request.Scheme.Replace("http:", "https:") : request.Scheme;
        return $"{schema}://{request.Host}/";
    }
    #endregion
}

