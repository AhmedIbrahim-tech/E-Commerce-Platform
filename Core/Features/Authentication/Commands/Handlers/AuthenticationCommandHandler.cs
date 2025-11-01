using Core.Features.Authentication.Commands.Models;
using Ecommerce.DataAccess.Services.OAuth;

namespace Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ApiResponseHandler,
        IRequestHandler<SignInCommand, ApiResponse<JwtAuthResponse>>,
        IRequestHandler<RefreshTokenCommand, ApiResponse<JwtAuthResponse>>,
        IRequestHandler<SendResetPasswordCommand, ApiResponse<string>>,
        IRequestHandler<ResetPasswordCommand, ApiResponse<string>>,
        IRequestHandler<GoogleLoginCommand, ApiResponse<JwtAuthResponse>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthGoogleService _authGoogleService;
        #endregion

        #region Constructors
        public AuthenticationCommandHandler(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthenticationService authenticationService,
            IAuthGoogleService authGoogleService) : base()
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _authGoogleService = authGoogleService;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null) return BadRequest<JwtAuthResponse>(SharedResourcesKeys.UserNameIsNotExist);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!user.EmailConfirmed) return BadRequest<JwtAuthResponse>(SharedResourcesKeys.EmailIsNotConfirmed);
            if (!signInResult.Succeeded) return BadRequest<JwtAuthResponse>(SharedResourcesKeys.InvalidPassword);

            var result = await _authenticationService.GetJWTTokenAsync(user);
            return Success(result);
        }

        public async Task<ApiResponse<JwtAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtToken = _authenticationService.ReadJwtToken(request.AccessToken);
            var userIdAndExpireDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
            switch (userIdAndExpireDate)
            {
                case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResponse>(SharedResourcesKeys.AlgorithmIsWrong);
                case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResponse>(SharedResourcesKeys.TokenIsNotExpired);
                case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResponse>(SharedResourcesKeys.RefreshTokenIsNotFound);
                case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResponse>(SharedResourcesKeys.RefreshTokenIsExpired);
            }
            var (userId, expiryDate) = userIdAndExpireDate;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound<JwtAuthResponse>();
            }
            var result = await _authenticationService.GetRefreshTokenAsync(user, jwtToken, expiryDate, request.RefreshToken);
            return Success(result);
        }

        public async Task<ApiResponse<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetPasswordResult = await _authenticationService.SendResetPasswordCodeAsync(request.Email);
            return resetPasswordResult switch
            {
                "Success" => Success(""),
                "UserNotFound" => BadRequest<string>(SharedResourcesKeys.UserNotFound),
                _ => BadRequest<string>(SharedResourcesKeys.TryAgainLater)
            };
        }

        public async Task<ApiResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetPasswordResult = await _authenticationService.ResetPasswordAsync(request.Email, request.NewPassword);
            return resetPasswordResult switch
            {
                "Success" => Success(""),
                "UserNotFound" => BadRequest<string>(SharedResourcesKeys.UserNotFound),
                _ => BadRequest<string>(SharedResourcesKeys.InvaildCode)
            };
        }

        public async Task<ApiResponse<JwtAuthResponse>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var (response, message) = await _authGoogleService.AuthenticateWithGoogleAsync(request.IdToken);
            return message switch
            {
                "Success" => Success(response),
                "InvalidGoogleToken" => BadRequest<JwtAuthResponse>(SharedResourcesKeys.InvalidGoogleToken),
                "FailedToAddNewRoles" => BadRequest<JwtAuthResponse>(SharedResourcesKeys.FailedToAddNewRoles),
                "FailedToAddNewClaims" => BadRequest<JwtAuthResponse>(SharedResourcesKeys.FailedToAddNewClaims),
                "GoogleAuthenticationFailed" => BadRequest<JwtAuthResponse>(SharedResourcesKeys.GoogleAuthenticationFailed),
                _ => BadRequest<JwtAuthResponse>(message),
            };
        }
        #endregion
    }
}
