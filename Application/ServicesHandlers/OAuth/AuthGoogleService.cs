using Infrastructure.Data.Authorization;

namespace Application.ServicesHandlers.OAuth;

public interface IAuthGoogleService
{
    Task<(JwtAuthResponse, string)> AuthenticateWithGoogleAsync(string idToken);
    Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);
}

public class AuthGoogleService : IAuthGoogleService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly GoogleAuthSettings _settings;
    private readonly IAuthenticationService _authenticationService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IDefaultClaimsService _defaultClaimsService;

    public AuthGoogleService(UserManager<AppUser> userManager,
        GoogleAuthSettings settings,
        IAuthenticationService authenticationService,
        ApplicationDbContext dbContext,
        IDefaultClaimsService defaultClaimsService)
    {
        _userManager = userManager;
        _settings = settings;
        _authenticationService = authenticationService;
        _dbContext = dbContext;
        _defaultClaimsService = defaultClaimsService;
    }

    public async Task<(JwtAuthResponse, string)> AuthenticateWithGoogleAsync(string idToken)
    {
        try
        {
            var payload = await ValidateGoogleTokenAsync(idToken);

            if (payload == null)
                return (null!, "InvalidGoogleToken");

            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                var fullName = $"{payload.GivenName} {payload.FamilyName}".Trim();
                var userName = payload.Email.Split('@')[0];
                var appUser = new AppUser(userName, fullName)
                {
                    Email = payload.Email,
                    EmailConfirmed = payload.EmailVerified,
                    PhoneNumber = "N/A"
                };

                var result = await _userManager.CreateAsync(appUser);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (null!, $"Failed to create user: {errors}");
                }

                var customer = new Customer(
                    appUserId: appUser.Id,
                    fullName: fullName,
                    gender: Gender.Unspecified,
                    createdBy: appUser.Id
                );

                await _dbContext.Customers.AddAsync(customer, CancellationToken.None);
                await _dbContext.SaveChangesAsync(CancellationToken.None);

                var addToRoleResult = await _userManager.AddToRoleAsync(appUser, Roles.Customer);
                if (!addToRoleResult.Succeeded)
                    return (null!, "FailedToAddNewRoles");

                var addDefaultClaimsResult = await _defaultClaimsService.AssignDefaultClaimsAsync(appUser, Roles.Customer);
                if (!addDefaultClaimsResult.Succeeded)
                    return (null!, "FailedToAddNewClaims");

                user = appUser;
            }
            else
            {
                if (payload.EmailVerified && !user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
            }
            var response = await _authenticationService.GetJWTTokenAsync(user);
            return (response, "Success");
        }
        catch (Exception)
        {
            return (null!, "GoogleAuthenticationFailed");
        }
    }

    public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
    {
        try
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _settings.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, validationSettings);

            if (payload == null || string.IsNullOrEmpty(payload.Email))
                throw new UnauthorizedAccessException("Invalid Google Token: Payload is null or missing email");

            return payload;
        }
        catch (InvalidJwtException ex)
        {
            throw new UnauthorizedAccessException("Invalid Google Token: " + ex.InnerException?.Message ?? ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to validate Google Token: {Message}" + ex.InnerException?.Message ?? ex.Message);
        }
    }
}

