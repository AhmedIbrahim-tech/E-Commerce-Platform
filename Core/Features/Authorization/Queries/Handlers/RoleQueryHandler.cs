using Core.Features.Authorization.Queries.Models;
using Core.Features.Authorization.Queries.Responses;

namespace Core.Features.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ApiResponseHandler,
        IRequestHandler<GetRoleByIdQuery, ApiResponse<GetSingleRoleResponse>>,
        IRequestHandler<GetRoleListQuery, ApiResponse<List<GetRoleListResponse>>>,
        IRequestHandler<ManageUserRolesQuery, ApiResponse<ManageUserRolesResponse>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public RoleQueryHandler(IAuthorizationService authorizationService, UserManager<User> userManager, IMapper mapper) : base()
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _mapper = mapper;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<GetSingleRoleResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleByIdAsync(request.Id);
            if (role is null) NotFound<GetSingleRoleResponse>();
            var roleMapper = _mapper.Map<GetSingleRoleResponse>(role);
            return Success(roleMapper);
        }

        public async Task<ApiResponse<List<GetRoleListResponse>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            var roleList = await _authorizationService.GetRolesListAsync();
            if (roleList is null) NotFound<GetRoleListResponse>();
            var roleListMapper = _mapper.Map<List<GetRoleListResponse>>(roleList);
            return Success(roleListMapper);
        }

        public async Task<ApiResponse<ManageUserRolesResponse>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null) return NotFound<ManageUserRolesResponse>();
            var result = await _authorizationService.ManageUserRolesData(user);
            return Success(result);
        }
        #endregion
    }
}
