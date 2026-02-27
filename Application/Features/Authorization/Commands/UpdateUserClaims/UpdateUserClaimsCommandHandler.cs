using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Authorization;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Infrastructure.Seeder;
using System.Security.Claims;

namespace Application.Features.Authorization.Commands.UpdateUserClaims;

public class UpdateUserClaimsCommandHandler(
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IUserCreationService userCreationService) : ApiResponseHandler(),
    IRequestHandler<UpdateUserClaimsCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
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

            var userRoles = await userManager.GetRolesAsync(user);
            var targetRole = userRoles.FirstOrDefault() ?? Roles.Customer;

            var creatorId = currentUserService.GetUserId();

            var selectedClaims = request.UserClaims
                .Where(x => x.Value == true)
                .Select(x => x.Type)
                .ToList();

            if (selectedClaims.Any())
            {
                var validationResult = await userCreationService.ValidateClaimAssignmentAsync(
                    creatorId,
                    targetRole,
                    selectedClaims);

                if (!validationResult.Succeeded)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return validationResult;
                }
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var removeResult = await userManager.RemoveClaimsAsync(user, userClaims);
            if (!removeResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(PermissionErrors.PermissionNotAssigned());
            }

            if (selectedClaims.Any())
            {
                var claims = selectedClaims.Select(claim => new Claim(claim, "True")).ToList();
                var addClaimsresult = await userManager.AddClaimsAsync(user, claims);
                if (!addClaimsresult.Succeeded)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<string>(PermissionErrors.PermissionAlreadyAssigned());
                }
            }

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<string>(PermissionErrors.InvalidPermissionScope());
        }
    }
}

