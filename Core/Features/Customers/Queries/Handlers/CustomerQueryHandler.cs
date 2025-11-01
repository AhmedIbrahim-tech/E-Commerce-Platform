using Core.Features.Customers.Queries.Models;
using Core.Features.Customers.Queries.Responses;

namespace Core.Features.Customers.Queries.Handlers
{
    public class CustomerQueryHandler : ApiResponseHandler,
        IRequestHandler<GetCustomerByIdQuery, ApiResponse<GetSingleCustomerResponse>>,
        IRequestHandler<GetCustomerPaginatedListQuery, PaginatedResult<GetCustomerPaginatedListResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;        public CustomerQueryHandler(UserManager<User> userManager, IMapper mapper) : base()
        {
            _userManager = userManager;
            _mapper = mapper;        }

        public async Task<ApiResponse<GetSingleCustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _userManager.Users.FirstOrDefaultAsync(e => e.Id.Equals(request.Id));
            if (customer is null) return NotFound<GetSingleCustomerResponse>(SharedResourcesKeys.NotFound);
            var customerMapper = _mapper.Map<GetSingleCustomerResponse>(customer);
            return Success(customerMapper);
        }

        public async Task<PaginatedResult<GetCustomerPaginatedListResponse>> Handle(GetCustomerPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Customer, GetCustomerPaginatedListResponse>> expression = c => new GetCustomerPaginatedListResponse
            {
                Id = c.Id,
                FullName = c.FirstName + " " + c.LastName,
                Email = c.Email,
                Gender = c.Gender,
                PhoneNumber = c.PhoneNumber
            };
            var customers = _userManager.Users.OfType<Customer>().AsQueryable().ApplyFiltering(request.SortBy, request.Search);
            var paginatedList = await customers.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }
    }
}
