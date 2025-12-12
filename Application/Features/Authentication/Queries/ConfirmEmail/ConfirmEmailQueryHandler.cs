
using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Authentication.ConfirmEmail;

public class ConfirmEmailQueryHandler : ApiResponseHandler,
    IRequestHandler<ConfirmEmailQuery, ApiResponse<string>>
{
    private readonly IAuthenticationService _authenticationService;

    public ConfirmEmailQueryHandler(IAuthenticationService authenticationService) : base()
    {
        _authenticationService = authenticationService;
    }

    public async Task<ApiResponse<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        var confirmEmailResult = await _authenticationService.ConfirmEmailAsync(request.UserId, request.Code);
        return confirmEmailResult switch
        {
            "UserOrCodeIsNullOrEmpty" => new ApiResponse<string>(UserErrors.InvalidCode()),
            "Success" => Success<string>("ConfirmEmailDone"),
            _ => new ApiResponse<string>(UserErrors.InvalidCode())
        };
    }
}

