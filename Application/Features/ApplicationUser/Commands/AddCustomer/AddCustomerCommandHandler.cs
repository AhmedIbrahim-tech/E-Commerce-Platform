using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.ApplicationUser.Commands.AddCustomer;

internal class AddCustomerCommandHandler(
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    ApplicationDbContext dbContext,
    IUrlHelper urlHelper) : ApiResponseHandler(),
    IRequestHandler<AddCustomerCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var appUser = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            FullName = $"{request.FirstName} {request.LastName}".Trim()
        };

        using var trans = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var userEmailIsExistResult = await userManager.FindByEmailAsync(appUser.Email!);
            if (userEmailIsExistResult != null)
            {
                await trans.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.DuplicatedEmail());
            }

            var userByUserName = await userManager.FindByNameAsync(appUser.UserName!);
            if (userByUserName != null)
            {
                await trans.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.DuplicatedEmail());
            }

            appUser.EmailConfirmed = true;

            var createResult = await userManager.CreateAsync(appUser, request.Password);
            if (!createResult.Succeeded)
            {
                await trans.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
            }

            var addToRoleResult = await userManager.AddToRoleAsync(appUser, "Customer");
            if (!addToRoleResult.Succeeded)
            {
                await trans.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(RoleErrors.InvalidPermissions());
            }

            var claims = new List<Claim>
            {
                new Claim("Edit Customer", "True"),
                new Claim("Get Customer", "True")
            };
            var addDefaultClaimsResult = await userManager.AddClaimsAsync(appUser, claims);
            if (!addDefaultClaimsResult.Succeeded)
            {
                await trans.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(PermissionErrors.PermissionNotAssigned());
            }

            var code = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var resquestAccessor = httpContextAccessor.HttpContext!.Request;
            var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host
                + urlHelper.Action("ConfirmEmail", "Authentication", new { userId = appUser.Id, code = code });

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                AppUserId = appUser.Id,
                FullName = $"{request.FirstName} {request.LastName}".Trim(),
                Gender = request.Gender
            };

            await dbContext.Customers.AddAsync(customer, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await trans.CommitAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            await trans.RollbackAsync(cancellationToken);
            return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
        }
    }
}

