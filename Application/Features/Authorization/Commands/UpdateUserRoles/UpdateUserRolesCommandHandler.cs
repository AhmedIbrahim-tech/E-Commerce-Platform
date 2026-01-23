using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Authorization;
using Infrastructure.Seeder;

namespace Application.Features.Authorization.Commands.UpdateUserRoles;

public class UpdateUserRolesCommandHandler(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IUserCreationService userCreationService) : ApiResponseHandler(), IRequestHandler<UpdateUserRolesCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var transact = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.UserNotFound());
            }

            // Get creator ID
            var creatorId = currentUserService.GetUserId();

            // Get selected roles from request
            var selectedRoles = (request.UserRoles ?? Enumerable.Empty<UserRoles>())
                .Where(x => x.HasRole == true && x.Name != null)
                .Select(x => x.Name!)
                .ToList();

            // Validate each role assignment
            foreach (var targetRole in selectedRoles)
            {
                var validationResult = await userCreationService.ValidateRoleAssignmentAsync(creatorId, targetRole);
                if (!validationResult.Succeeded)
                {
                    await transact.RollbackAsync(cancellationToken);
                    return validationResult;
                }
            }

            // Remove existing roles
            var userRoles = await userManager.GetRolesAsync(user);
            var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(RoleErrors.RoleNotAssigned());
            }

            // Add new roles (validated above)
            if (selectedRoles.Any())
            {
                var addRolesresult = await userManager.AddToRolesAsync(user, selectedRoles);
                if (!addRolesresult.Succeeded)
                {
                    await transact.RollbackAsync(cancellationToken);
                    return new ApiResponse<string>(RoleErrors.InvalidPermissions());
                }

                // Update claims based on new roles
                // Remove all existing claims
                var existingClaims = await userManager.GetClaimsAsync(user);
                if (existingClaims.Any())
                {
                    await userManager.RemoveClaimsAsync(user, existingClaims);
                }

                // Assign default claims for each role
                foreach (var role in selectedRoles)
                {
                    var defaultClaims = Permissions.GetDefaultForRole(role);
                    if (defaultClaims.Any())
                    {
                        var claims = defaultClaims.Select(claim => new System.Security.Claims.Claim(claim, "True")).ToList();
                        await userManager.AddClaimsAsync(user, claims);
                    }
                }
            }

            await transact.CommitAsync(cancellationToken);
            
            await UserSeeder.SyncAllClaimsToDefaultUserAsync(userManager);
            
            return Edit("");
        }
        catch (Exception)
        {
            await transact.RollbackAsync(cancellationToken);
            return new ApiResponse<string>(RoleErrors.RoleAlreadyAssigned());
        }
    }
}

