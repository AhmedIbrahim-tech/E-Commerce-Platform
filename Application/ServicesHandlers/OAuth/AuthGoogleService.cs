using Application.Common.Settings;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

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

    public AuthGoogleService(UserManager<AppUser> userManager,
        GoogleAuthSettings settings,
        IAuthenticationService authenticationService,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _settings = settings;
        _authenticationService = authenticationService;
        _dbContext = dbContext;
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
                var appUser = new AppUser
                {
                    UserName = payload.Email.Split('@')[0],
                    Email = payload.Email,
                    EmailConfirmed = payload.EmailVerified,
                    PhoneNumber = "N/A",
                    FullName = $"{payload.GivenName} {payload.FamilyName}".Trim()
                };

                var result = await _userManager.CreateAsync(appUser);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (null!, $"Failed to create user: {errors}");
                }

                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    AppUserId = appUser.Id,
                    FullName = $"{payload.GivenName} {payload.FamilyName}".Trim(),
                    Gender = Gender.Unspecified
                };

                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();

                var addToRoleResult = await _userManager.AddToRoleAsync(appUser, "Customer");
                if (!addToRoleResult.Succeeded)
                    return (null!, "FailedToAddNewRoles");

                var claims = new List<Claim>
                {
                    new Claim("Edit Customer", "True"),
                    new Claim("Get Customer", "True")
                };
                var addDefaultClaimsResult = await _userManager.AddClaimsAsync(appUser, claims);
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

