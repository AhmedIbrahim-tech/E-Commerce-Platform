using Application.Common.Bases;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Domain.Entities.Users;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ApplicationUser.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetUserByIdQuery, ApiResponse<GetUserByIdResponse>>
{
    public async Task<ApiResponse<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await (from admin in unitOfWork.Admins.GetTableNoTracking()
                               where admin.Id == request.Id
                               select new { admin.AppUserId, admin.Id, admin.FullName, admin.Gender, admin.SecondPhoneNumber, admin.Address, Type = "Admin" })
                               .Union(from vendor in unitOfWork.Vendors.GetTableNoTracking()
                                      where vendor.Id == request.Id
                                      select new { vendor.AppUserId, vendor.Id, vendor.FullName, vendor.Gender, vendor.SecondPhoneNumber, Address = (string?)null, Type = "Vendor" })
                               .Union(from customer in unitOfWork.Customers.GetTableNoTracking()
                                      where customer.Id == request.Id
                                      select new { customer.AppUserId, customer.Id, customer.FullName, customer.Gender, customer.SecondPhoneNumber, Address = (string?)null, Type = "Customer" })
                               .FirstOrDefaultAsync(cancellationToken);

        Guid appUserId;
        Guid entityId;
        string? fullName;
        Gender? gender = null;
        string? secondPhoneNumber = null;
        string? address = null;

        if (userInfo != null)
        {
            appUserId = userInfo.AppUserId;
            entityId = userInfo.Id;
            fullName = userInfo.FullName;
            gender = userInfo.Gender;
            secondPhoneNumber = userInfo.SecondPhoneNumber;
            address = userInfo.Address;
        }
        else
        {
            appUserId = request.Id;
            entityId = request.Id;
            fullName = null;
        }

        var appUser = await userManager.FindByIdAsync(appUserId.ToString());
        if (appUser == null)
            return NotFound<GetUserByIdResponse>("User not found");

        if (userInfo == null)
        {
            entityId = appUser.Id;
            fullName = appUser.DisplayName;
        }

        var roles = await userManager.GetRolesAsync(appUser);
        var role = roles.FirstOrDefault() ?? "Customer";

        var claims = await userManager.GetClaimsAsync(appUser);
        var claimValues = claims.Select(c => c.Value).ToList();

        var isActive = appUser.LockoutEnd == null || appUser.LockoutEnd <= DateTimeOffset.UtcNow;

        var lastLogin = await unitOfWork.RefreshTokens.GetTableNoTracking()
            .Where(t => t.AppUserId == appUser.Id)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => t.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var response = new GetUserByIdResponse
        {
            Id = entityId,
            AppUserId = appUser.Id,
            FullName = fullName ?? appUser.DisplayName,
            UserName = appUser.UserName,
            Email = appUser.Email,
            PhoneNumber = appUser.PhoneNumber,
            SecondPhoneNumber = secondPhoneNumber,
            Gender = gender,
            Address = address,
            Role = role,
            Roles = roles.ToList(),
            ProfileImage = fileUploadService.ToAbsoluteUrl(appUser.ProfileImage),
            IsActive = isActive,
            CreatedAt = null,
            LastLoginAt = lastLogin != default ? lastLogin : null,
            Claims = claimValues
        };

        return Success(response);
    }
}
