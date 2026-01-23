using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Commands.ToggleCustomerStatus;

public class ToggleCustomerStatusCommandHandler : ApiResponseHandler,
    IRequestHandler<ToggleCustomerStatusCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ToggleCustomerStatusCommandHandler(
        ApplicationDbContext dbContext, 
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<string>> Handle(ToggleCustomerStatusCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .IgnoreQueryFilters()
            .AsTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<string>(CustomerErrors.CustomerNotFound());

        var appUser = await _userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var currentUserId = _currentUserService.GetUserId();
        
        // Toggle status: if deleted, restore; if active, mark as deleted
        if (customer.IsDeleted)
        {
            customer.Restore(currentUserId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Success("Customer activated successfully");
        }
        else
        {
            customer.MarkDeleted(currentUserId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Success("Customer deactivated successfully");
        }
    }
}
