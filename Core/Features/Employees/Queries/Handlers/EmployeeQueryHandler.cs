using Core.Features.Employees.Queries.Models;
using Core.Features.Employees.Queries.Responses;

namespace Core.Features.Employees.Queries.Handlers
{
    public class EmployeeQueryHandler : ApiResponseHandler,
        IRequestHandler<GetEmployeeByIdQuery, ApiResponse<GetSingleEmployeeResponse>>,
        IRequestHandler<GetEmployeePaginatedListQuery, PaginatedResult<GetEmployeePaginatedListResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;        public EmployeeQueryHandler(UserManager<User> userManager, IMapper mapper) : base()
        {
            _userManager = userManager;
            _mapper = mapper;        }

        public async Task<ApiResponse<GetSingleEmployeeResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _userManager.Users.FirstOrDefaultAsync(e => e.Id.Equals(request.Id));
            if (employee is null) return NotFound<GetSingleEmployeeResponse>(SharedResourcesKeys.NotFound);
            var employeeMapper = _mapper.Map<GetSingleEmployeeResponse>(employee);
            return Success(employeeMapper);
        }

        public async Task<PaginatedResult<GetEmployeePaginatedListResponse>> Handle(GetEmployeePaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Employee, GetEmployeePaginatedListResponse>> expression = c => new GetEmployeePaginatedListResponse
            {
                Id = c.Id,
                FullName = c.FirstName + " " + c.LastName,
                Email = c.Email,
                Gender = c.Gender,
                PhoneNumber = c.PhoneNumber,
                Position = c.Position,
                Salary = c.Salary,
                HireDate = c.HireDate,
                Address = c.Address
            };
            var employees = _userManager.Users.OfType<Employee>().AsQueryable().ApplyFiltering(request.SortBy, request.Search);
            var paginatedList = await employees.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }
    }
}
