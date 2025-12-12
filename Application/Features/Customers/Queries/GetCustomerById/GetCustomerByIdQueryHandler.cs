using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler : ApiResponseHandler,
    IRequestHandler<GetCustomerByIdQuery, ApiResponse<GetCustomerByIdResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public GetCustomerByIdQueryHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<GetCustomerByIdResponse>(CustomerErrors.CustomerNotFound());

        var appUser = await _userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<GetCustomerByIdResponse>(UserErrors.UserNotFound());

        var customerResponse = new GetCustomerByIdResponse
        {
            Id = customer.Id,
            FullName = customer.FullName ?? string.Empty,
            Email = appUser.Email ?? string.Empty,
            PhoneNumber = appUser.PhoneNumber ?? string.Empty,
            Gender = customer.Gender
        };

        return Success(customerResponse);
    }
}

