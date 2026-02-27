using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<GetCustomerByIdQuery, ApiResponse<GetCustomerByIdResponse>>
{
    public async Task<ApiResponse<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetTableNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<GetCustomerByIdResponse>(CustomerErrors.CustomerNotFound());

        var appUser = await userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<GetCustomerByIdResponse>(UserErrors.UserNotFound());

        var customerResponse = new GetCustomerByIdResponse
        {
            Id = customer.Id,
            FullName = customer.FullName ?? string.Empty,
            UserName = appUser.UserName ?? string.Empty,
            Email = appUser.Email ?? string.Empty,
            PhoneNumber = appUser.PhoneNumber ?? string.Empty,
            Gender = customer.Gender
        };

        return Success(customerResponse);
    }
}

