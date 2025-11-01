using Core.Features.Employees.Commands.Models;

namespace Core.Features.Employees.Commands.Handlers
{
    public class EmployeeCommandHandler : ApiResponseHandler,
        IRequestHandler<AddEmployeeCommand, ApiResponse<string>>,
        IRequestHandler<EditEmployeeCommand, ApiResponse<string>>,
        IRequestHandler<DeleteEmployeeCommand, ApiResponse<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;        public EmployeeCommandHandler(UserManager<User> userManager, IMapper mapper) : base()
        {
            _userManager = userManager;
            _mapper = mapper;        }

        public async Task<ApiResponse<string>> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) return BadRequest<string>(SharedResourcesKeys.EmailIsExist);

            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName != null) return BadRequest<string>(SharedResourcesKeys.UserNameIsExist);

            var identityUser = _mapper.Map<Employee>(request);
            var createResult = await _userManager.CreateAsync(identityUser, request.Password);
            if (!createResult.Succeeded)
                return BadRequest<string>(createResult.Errors.FirstOrDefault().Description);

            //Add default role "Employee"
            var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, "Employee");
            if (!addToRoleResult.Succeeded)
                return BadRequest<string>(SharedResourcesKeys.FailedToAddNewRoles);

            //Add default employee policies
            var claims = new List<Claim>
            {
                new Claim("Edit Employee", "True"),
                new Claim("Get Employee", "True")
            };
            var addDefaultClaimsResult = await _userManager.AddClaimsAsync(identityUser, claims);
            if (!addDefaultClaimsResult.Succeeded)
                return BadRequest<string>(SharedResourcesKeys.FailedToAddNewClaims);

            return Created("");
        }

        public async Task<ApiResponse<string>> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            var oldEmployee = await _userManager.FindByIdAsync(request.Id.ToString());
            if (oldEmployee is null) return NotFound<string>();

            var isUserNameDuplicate = await _userManager.UserNameExistsAsync(request.UserName, request.Id);
            if (isUserNameDuplicate)
                return BadRequest<string>(SharedResourcesKeys.UserNameIsExist);

            var isEmailDuplicate = await _userManager.EmailExistsAsync(request.Email, request.Id);
            if (isEmailDuplicate)
                return BadRequest<string>(SharedResourcesKeys.EmailIsExist);

            var newEmployee = _mapper.Map(request, oldEmployee);
            var updateResult = await _userManager.UpdateAsync(newEmployee);

            if (!updateResult.Succeeded)
                return BadRequest<string>(SharedResourcesKeys.UpdateFailed);
            return Edit("");
        }

        public async Task<ApiResponse<string>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var customer = await _userManager.FindByIdAsync(request.Id.ToString());
            if (customer is null) return NotFound<string>();

            var deleteResult = await _userManager.DeleteAsync(customer);
            if (!deleteResult.Succeeded)
                return BadRequest<string>(SharedResourcesKeys.DeleteFailed);
            return Deleted<string>();
        }
    }
}
