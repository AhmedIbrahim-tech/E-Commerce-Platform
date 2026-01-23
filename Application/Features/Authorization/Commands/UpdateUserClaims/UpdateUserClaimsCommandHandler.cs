using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Infrastructure.Data;
using Infrastructure.Data.Authorization;
using Infrastructure.Data.Identity;
using Infrastructure.Seeder;
using System.Security.Claims;

namespace Application.Features.Authorization.Commands.UpdateUserClaims;

public class UpdateUserClaimsCommandHandler(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IUserCreationService userCreationService) : ApiResponseHandler(),
    IRequestHandler<UpdateUserClaimsCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
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

            // Get target user's role (for validation)
            var userRoles = await userManager.GetRolesAsync(user);
            var targetRole = userRoles.FirstOrDefault() ?? Roles.Customer; // Default to Customer if no role

            // Get creator ID
            var creatorId = currentUserService.GetUserId();

            // Get selected claims from request
            var selectedClaims = request.UserClaims
                .Where(x => x.Value == true)
                .Select(x => x.Type)
                .ToList();

            // Validate claim assignments
            if (selectedClaims.Any())
            {
                var validationResult = await userCreationService.ValidateClaimAssignmentAsync(
                    creatorId,
                    targetRole,
                    selectedClaims);

                if (!validationResult.Succeeded)
                {
                    await transact.RollbackAsync(cancellationToken);
                    return validationResult;
                }
            }

            // Remove existing claims
            var userClaims = await userManager.GetClaimsAsync(user);
            var removeResult = await userManager.RemoveClaimsAsync(user, userClaims);
            if (!removeResult.Succeeded)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(PermissionErrors.PermissionNotAssigned());
            }

            // Add new claims (validated above)
            if (selectedClaims.Any())
            {
                var claims = selectedClaims.Select(claim => new Claim(claim, "True")).ToList();
                var addClaimsresult = await userManager.AddClaimsAsync(user, claims);
                if (!addClaimsresult.Succeeded)
                {
                    await transact.RollbackAsync(cancellationToken);
                    return new ApiResponse<string>(PermissionErrors.PermissionAlreadyAssigned());
                }
            }

            await transact.CommitAsync(cancellationToken);
            
            await UserSeeder.SyncAllClaimsToDefaultUserAsync(userManager);
            
            return Edit("");
        }
        catch (Exception)
        {
            await transact.RollbackAsync(cancellationToken);
            return new ApiResponse<string>(PermissionErrors.InvalidPermissionScope());
        }
    }
}

