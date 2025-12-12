using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Commands.UpdateUserClaims;

public class UpdateUserClaimsCommandHandler(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext) : ApiResponseHandler(),
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

            var userClaims = await userManager.GetClaimsAsync(user);
            var removeResult = await userManager.RemoveClaimsAsync(user, userClaims);
            if (!removeResult.Succeeded)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(PermissionErrors.PermissionNotAssigned());
            }

            var selectedClaims = request.UserClaims.Where(x => x.Value == true).Select(x => new Claim(x.Type, x.Value.ToString()));
            var addClaimsresult = await userManager.AddClaimsAsync(user, selectedClaims);
            if (!addClaimsresult.Succeeded)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(PermissionErrors.PermissionAlreadyAssigned());
            }

            await transact.CommitAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            await transact.RollbackAsync(cancellationToken);
            return new ApiResponse<string>(PermissionErrors.InvalidPermissionScope());
        }
    }
}

