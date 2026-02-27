using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Authorization;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Infrastructure.Seeder;

namespace Application.Features.Authorization.Commands.UpdateUserRoles;

public class UpdateUserRolesCommandHandler(
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IUserCreationService userCreationService) : ApiResponseHandler(), IRequestHandler<UpdateUserRolesCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        using var transact = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.UserNotFound());
            }

            var creatorId = currentUserService.GetUserId();

            var selectedRoles = (request.UserRoles ?? Enumerable.Empty<UserRoles>())
                .Where(x => x.HasRole == true && x.Name != null)
                .Select(x => x.Name!)
                .ToList();

            foreach (var targetRole in selectedRoles)
            {
                var validationResult = await userCreationService.ValidateRoleAssignmentAsync(creatorId, targetRole);
                if (!validationResult.Succeeded)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return validationResult;
                }
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(RoleErrors.RoleNotAssigned());
            }

            if (selectedRoles.Any())
            {
                var addRolesresult = await userManager.AddToRolesAsync(user, selectedRoles);
                if (!addRolesresult.Succeeded)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<string>(RoleErrors.InvalidPermissions());
                }

                var existingClaims = await userManager.GetClaimsAsync(user);
                if (existingClaims.Any())
                {
                    await userManager.RemoveClaimsAsync(user, existingClaims);
                }

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

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<string>(RoleErrors.RoleAlreadyAssigned());
        }
    }
}

