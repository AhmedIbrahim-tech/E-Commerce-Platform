using Application.ServicesHandlers.Services;
using Infrastructure.Data.Authorization;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.ApplicationUser.Commands.AddCustomer;

internal class AddCustomerCommandHandler(
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IUrlHelper urlHelper,
    IDefaultClaimsService defaultClaimsService,
    INotificationService notificationService,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<AddCustomerCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var fullName = request.FullName?.Trim() ?? string.Empty;
        var appUser = new AppUser(request.UserName, fullName)
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        appUser.Id = Guid.NewGuid();

        await using var trans = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var userEmailIsExistResult = await userManager.FindByEmailAsync(appUser.Email!);
            if (userEmailIsExistResult != null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.DuplicatedEmail());
            }

            var userByUserName = await userManager.FindByNameAsync(appUser.UserName!);
            if (userByUserName != null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.DuplicatedUserName());
            }

            appUser.EmailConfirmed = true;

            var createResult = await userManager.CreateAsync(appUser, request.Password);
            if (!createResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
            }

            if (request.ProfileImage != null)
            {
                var profileImageUrls = await fileUploadService.UploadAndGetUrlsAsync(
                    new[] { request.ProfileImage },
                    FileLocations.Users,
                    appUser.Id,
                    childFolder: null,
                    overwrite: true,
                    cancellationToken: cancellationToken);

                if (profileImageUrls.Count > 0)
                {
                    appUser.ProfileImage = profileImageUrls[0];
                    var picUpdate = await userManager.UpdateAsync(appUser);
                    if (!picUpdate.Succeeded)
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
                    }
                }
            }

            var addToRoleResult = await userManager.AddToRoleAsync(appUser, Roles.Customer);
            if (!addToRoleResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(RoleErrors.InvalidPermissions());
            }

            var addDefaultClaimsResult = await defaultClaimsService.AssignDefaultClaimsAsync(appUser, Roles.Customer);
            if (!addDefaultClaimsResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(PermissionErrors.PermissionNotAssigned());
            }

            var code = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var resquestAccessor = httpContextAccessor.HttpContext!.Request;
            var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host
                + urlHelper.Action("ConfirmEmail", "Authentication", new { userId = appUser.Id, code = code });

            var customer = new Customer(
                appUserId: appUser.Id,
                fullName: fullName,
                gender: request.Gender ?? Gender.Unspecified,
                createdBy: appUser.Id
            );

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                customer.ChangePhoneNumber(request.PhoneNumber, appUser.Id);
            }

            if (!string.IsNullOrWhiteSpace(request.SecondPhoneNumber))
            {
                customer.ChangeSecondPhoneNumber(request.SecondPhoneNumber, appUser.Id);
            }

            await unitOfWork.Customers.AddAsync(customer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await notificationService.CreateAsync(
                "new_user",
                new { userId = appUser.Id, userName = appUser.UserName, role = Roles.Customer },
                new NotificationRecipients(),
                cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
        }
    }
}

