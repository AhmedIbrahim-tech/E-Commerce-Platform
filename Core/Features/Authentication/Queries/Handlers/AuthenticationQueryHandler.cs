using Core.Features.Authentication.Queries.Models;

namespace Core.Features.Authentication.Queries.Handlers
{
    public class AuthenticationQueryHandler : ApiResponseHandler,
        IRequestHandler<AuthorizeUserQuery, ApiResponse<string>>,
        IRequestHandler<ConfirmEmailQuery, ApiResponse<string>>,
        IRequestHandler<ConfirmResetPasswordQuery, ApiResponse<string>>
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        #endregion

        #region Constructors
        public AuthenticationQueryHandler(IAuthenticationService authenticationService) : base()
        {
            _authenticationService = authenticationService;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ValidateToken(request.AccessToken);
            if (result == "NotExpired")
                return Success(result);
            return Unauthorized<string>(SharedResourcesKeys.TokenIsExpired);
        }

        public async Task<ApiResponse<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var confirmEmailResult = await _authenticationService.ConfirmEmailAsync(request.UserId, request.Code);
            return confirmEmailResult switch
            {
                "UserOrCodeIsNullOrEmpty" => BadRequest<string>(SharedResourcesKeys.UserOrCodeIsNullOrEmpty),
                "Success" => Success<string>(SharedResourcesKeys.ConfirmEmailDone),
                _ => BadRequest<string>(confirmEmailResult)
            };
        }

        public async Task<ApiResponse<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var confirmResetPasswordResult = await _authenticationService.ConfirmResetPasswordAsync(request.Code, request.Email);
            return confirmResetPasswordResult switch
            {
                "UserNotFound" => BadRequest<string>(SharedResourcesKeys.UserNotFound),
                "Success" => Success(""),
                _ => BadRequest<string>(SharedResourcesKeys.InvaildCode)
            };
        }
        #endregion
    }
}
