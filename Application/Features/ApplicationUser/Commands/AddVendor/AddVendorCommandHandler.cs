using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Domain.Entities.Users;
using Infrastructure.Data.Authorization;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ApplicationUser.Commands.AddVendor;

internal class AddVendorCommandHandler(
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IUserCreationService userCreationService,
    IFileUploadService fileUploadService,
    INotificationService notificationService) : ApiResponseHandler(),
    IRequestHandler<AddVendorCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddVendorCommand request, CancellationToken cancellationToken)
    {
        var fullName = $"{request.FirstName} {request.LastName}".Trim();
        var appUser = new AppUser(request.UserName, fullName)
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true
        };

        appUser.Id = Guid.NewGuid();
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
                appUser.ProfileImage = profileImageUrls[0];
        }

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

            var creatorId = currentUserService.GetUserId();
            var creator = await userManager.FindByIdAsync(creatorId.ToString());
            if (creator == null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.UserNotFound());
            }

            var creatorRoles = await userManager.GetRolesAsync(creator);
            var creatorRole = creatorRoles.FirstOrDefault();

            string storeName;
            decimal commissionRate;

            if (creatorRole == Roles.Merchant)
            {
                var creatorVendor = await unitOfWork.Vendors.GetTableNoTracking()
                    .FirstOrDefaultAsync(v => v.AppUserId == creatorId, cancellationToken);
                
                if (creatorVendor == null)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
                }

                storeName = creatorVendor.StoreName;
                commissionRate = creatorVendor.CommissionRate;
            }
            else
            {
                storeName = request.StoreName;
                commissionRate = request.CommissionRate;
                
                if (string.IsNullOrWhiteSpace(storeName))
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
                }
                
                if (commissionRate <= 0 || commissionRate >= 100)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
                }
            }

            const string targetRole = Roles.Merchant;

            var roleValidation = await userCreationService.ValidateRoleAssignmentAsync(creatorId, targetRole);
            if (!roleValidation.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return roleValidation;
            }

            var createResult = await userCreationService.CreateUserWithRoleAsync(
                appUser,
                request.Password!,
                targetRole,
                creatorId);

            if (!createResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
            }

            var vendor = new Vendor(
                appUserId: appUser.Id,
                ownerName: fullName,
                gender: request.Gender ?? Gender.Unspecified,
                storeName: storeName,
                commissionRate: commissionRate,
                createdBy: creatorId
            );

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                vendor.ChangePhoneNumber(request.PhoneNumber, creatorId);
            }

            if (!string.IsNullOrWhiteSpace(request.SecondPhoneNumber))
            {
                vendor.ChangeSecondPhoneNumber(request.SecondPhoneNumber, creatorId);
            }

            await unitOfWork.Vendors.AddAsync(vendor, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await notificationService.CreateAsync(
                "merchant_registered",
                new { merchantId = appUser.Id, storeName, commissionRate },
                new NotificationRecipients(
                    AdminIds: creatorRole == Roles.Admin ? new[] { creatorId } : null,
                    MerchantIds: new[] { appUser.Id }),
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
