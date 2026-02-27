using Infrastructure.Data.Authorization;
using Infrastructure.Seeder;

namespace Application.ServicesHandlers.Services;

public interface IUserCreationService
{
    Task<IdentityResult> CreateUserWithRoleAsync(AppUser appUser, string password, string targetRole, Guid creatorId, List<string>? customClaims = null);

    Task<ApiResponse<string>> ValidateRoleAssignmentAsync(Guid creatorId, string targetRole);

    Task<ApiResponse<string>> ValidateClaimAssignmentAsync(Guid creatorId, string targetRole, List<string> claims);

    List<string> GetAllowedClaimsForRole(string creatorRole, string targetRole);
}


public class UserCreationService(UserManager<AppUser> userManager, IDefaultClaimsService defaultClaimsService) : IUserCreationService
{
    public async Task<IdentityResult> CreateUserWithRoleAsync(AppUser appUser,string password,string targetRole,Guid creatorId,List<string>? customClaims = null)
    {
        var createResult = await userManager.CreateAsync(appUser, password);
        if (!createResult.Succeeded)
            return createResult;

        var addToRoleResult = await userManager.AddToRoleAsync(appUser, targetRole);
        if (!addToRoleResult.Succeeded)
        {
            await userManager.DeleteAsync(appUser);
            return addToRoleResult;
        }

        var defaultClaimsResult = await defaultClaimsService.AssignDefaultClaimsAsync(appUser, targetRole);
        if (!defaultClaimsResult.Succeeded)
        {
            await userManager.DeleteAsync(appUser);
            return defaultClaimsResult;
        }

        if (customClaims != null && customClaims.Any())
        {
            var claims = customClaims.Select(claim => new Claim(claim, "True")).ToList();
            var addClaimsResult = await userManager.AddClaimsAsync(appUser, claims);
            if (!addClaimsResult.Succeeded)
            {
                await userManager.DeleteAsync(appUser);
                return addClaimsResult;
            }
        }



        return IdentityResult.Success;
    }

    public async Task<ApiResponse<string>> ValidateRoleAssignmentAsync(Guid creatorId, string targetRole)
    {
        var creator = await userManager.FindByIdAsync(creatorId.ToString());
        if (creator == null)
            return new ApiResponse<string>(UserErrors.UserNotFound());

        var creatorRoles = await userManager.GetRolesAsync(creator);
        var creatorRole = creatorRoles.FirstOrDefault();

        if (string.IsNullOrEmpty(creatorRole))
            return new ApiResponse<string>(RoleErrors.InvalidPermissions());

        return creatorRole switch
        {
            Roles.SuperAdmin => ValidateSuperAdminCanCreate(targetRole),
            Roles.Admin => ValidateAdminCanCreate(targetRole),
            Roles.Merchant => ValidateMerchantCanCreate(targetRole),
            Roles.StaffMerchant => ValidateMerchantCanCreate(targetRole),
            _ => new ApiResponse<string>(RoleErrors.InvalidPermissions())
        };
    }

    public async Task<ApiResponse<string>> ValidateClaimAssignmentAsync(
        Guid creatorId,
        string targetRole,
        List<string> claims)
    {
        var creator = await userManager.FindByIdAsync(creatorId.ToString());
        if (creator == null)
            return new ApiResponse<string>(UserErrors.UserNotFound());

        var creatorRoles = await userManager.GetRolesAsync(creator);
        var creatorRole = creatorRoles.FirstOrDefault();

        if (string.IsNullOrEmpty(creatorRole))
            return new ApiResponse<string>(RoleErrors.InvalidPermissions());

        var allowedClaims = GetAllowedClaimsForRole(creatorRole, targetRole);
        var allPermissions = Permissions.GetAll();

        foreach (var claim in claims)
        {
            if (!allPermissions.Contains(claim))
                return new ApiResponse<string>(PermissionErrors.InvalidPermissionScope());

            if (!allowedClaims.Contains(claim))
                return new ApiResponse<string>(PermissionErrors.PermissionNotAssigned());
        }

        return new ApiResponse<string> { Succeeded = true };
    }

    public List<string> GetAllowedClaimsForRole(string creatorRole, string targetRole)
    {
        var allPermissions = Permissions.GetAll();

        return (creatorRole, targetRole) switch
        {
            (Roles.SuperAdmin, _) => allPermissions,

            (Roles.Admin, Roles.Admin) => Permissions.GetDefaultForRole(Roles.Admin),
            (Roles.Admin, Roles.Merchant) => Permissions.GetDefaultForRole(Roles.Merchant),
            (Roles.Admin, Roles.StaffMerchant) => Permissions.GetDefaultForRole(Roles.StaffMerchant),
            (Roles.Admin, Roles.Customer) => Permissions.GetDefaultForRole(Roles.Customer),

            (Roles.Merchant, Roles.Merchant) => GetMerchantAllowedClaims(),
            (Roles.Merchant, Roles.StaffMerchant) => GetMerchantAllowedClaims(),

            _ => new List<string>()
        };
    }

    #region Helper Methods
    private List<string> GetMerchantAllowedClaims()
    {
        return new List<string>
        {
            Permissions.Vendor.ViewProfile,
            Permissions.Vendor.EditProfile,
            Permissions.Vendor.ViewDashboard,
            Permissions.Auth.ChangePassword,
            Permissions.Auth.ViewOwnProfile,
            Permissions.Auth.EditOwnProfile
        };
    }

    private ApiResponse<string> ValidateSuperAdminCanCreate(string targetRole)
    {
        var allowedRoles = new[] { Roles.SuperAdmin, Roles.Admin, Roles.Merchant, Roles.StaffMerchant, Roles.Customer };
        if (!allowedRoles.Contains(targetRole))
            return new ApiResponse<string>(RoleErrors.InvalidPermissions());

        return new ApiResponse<string> { Succeeded = true };
    }

    private ApiResponse<string> ValidateAdminCanCreate(string targetRole)
    {
        var allowedRoles = new[] { Roles.Admin, Roles.Merchant, Roles.StaffMerchant, Roles.Customer };
        if (!allowedRoles.Contains(targetRole))
            return new ApiResponse<string>(RoleErrors.InvalidPermissions());

        return new ApiResponse<string> { Succeeded = true };
    }

    private ApiResponse<string> ValidateMerchantCanCreate(string targetRole)
    {
        if (targetRole != Roles.Merchant && targetRole != Roles.StaffMerchant)
            return new ApiResponse<string>(RoleErrors.InvalidPermissions());

        return new ApiResponse<string> { Succeeded = true };
    } 
    #endregion
}
