using Application.Common.Bases;
using Application.Common.Errors;
using Application.Common.Helpers;
using Domain.Responses;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Queries.ManageUserClaims;

public class ManageUserClaimsQueryHandler(UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<ManageUserClaimsQuery, ApiResponse<ManageUserClaimsResponse>>
{
    public async Task<ApiResponse<ManageUserClaimsResponse>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return new ApiResponse<ManageUserClaimsResponse>(UserErrors.UserNotFound());

        var response = new ManageUserClaimsResponse();
        var userClaimList = new List<UserClaims>();
        var userClaims = await userManager.GetClaimsAsync(user);
        response.UserId = user.Id;
        response.UserClaims = ClaimsStore.claims.Select(claim => new UserClaims
        {
            Type = claim.Type,
            Value = userClaims.Any(x => x.Type == claim.Type)
        }).ToList();

        return Success(response);
    }
}

