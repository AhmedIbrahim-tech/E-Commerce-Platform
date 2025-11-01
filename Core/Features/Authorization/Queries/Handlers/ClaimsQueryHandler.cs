using Core.Features.Authorization.Queries.Models;

namespace Core.Features.Authorization.Queries.Handlers
{
    public class ClaimsQueryHandler : ApiResponseHandler,
        IRequestHandler<ManageUserClaimsQuery, ApiResponse<ManageUserClaimsResponse>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public ClaimsQueryHandler(IAuthorizationService authorizationService, UserManager<User> userManager, IMapper mapper) : base()
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _mapper = mapper;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<ManageUserClaimsResponse>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null) return NotFound<ManageUserClaimsResponse>();
            var result = await _authorizationService.ManageUserClaimsData(user);
            return Success(result);
        }
        #endregion
    }
}
