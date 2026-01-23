namespace Application.Features.Authorization.Queries.ManageUserClaims;

public class ManageUserClaimsQueryHandler(UserManager<AppUser> userManager) : ApiResponseHandler(), IRequestHandler<ManageUserClaimsQuery, ApiResponse<ManageUserClaimsResponse>>
{
    public async Task<ApiResponse<ManageUserClaimsResponse>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return new ApiResponse<ManageUserClaimsResponse>(UserErrors.UserNotFound());

        var response = new ManageUserClaimsResponse();
        var userClaims = await userManager.GetClaimsAsync(user);
        response.UserId = user.Id;
        response.UserClaims = ClaimsStore.GetAllClaims().Select(claim => new UserClaims
        {
            Type = claim.Type,
            Value = userClaims.Any(x => x.Type == claim.Type && x.Value == "True")
        }).ToList();

        return Success(response);
    }
}

