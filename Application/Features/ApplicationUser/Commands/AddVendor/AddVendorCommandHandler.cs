using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Authorization;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ApplicationUser.Commands.AddVendor;

internal class AddVendorCommandHandler(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext,
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
            EmailConfirmed = true // Vendor users are created by admins/vendors, so email is trusted
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

        using var trans = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Validate email and username uniqueness
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

            // Get creator ID
            var creatorId = currentUserService.GetUserId();
            var creator = await userManager.FindByIdAsync(creatorId.ToString());
            if (creator == null)
            {
                await trans.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.UserNotFound());
            }

            var creatorRoles = await userManager.GetRolesAsync(creator);
            var creatorRole = creatorRoles.FirstOrDefault();

            // Determine store information based on creator
            string storeName;
            decimal commissionRate;

            if (creatorRole == Roles.Merchant)
            {
                // Vendor owner creating staff - use same store info
                var creatorVendor = await dbContext.Vendors
                    .FirstOrDefaultAsync(v => v.AppUserId == creatorId, cancellationToken);
                
                if (creatorVendor == null)
                {
                    await trans.RollbackAsync(cancellationToken);
                    return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
                }

                storeName = creatorVendor.StoreName;
                commissionRate = creatorVendor.CommissionRate;
            }
            else
            {
                // Admin/SuperAdmin creating new vendor - use provided info
                storeName = request.StoreName;
                commissionRate = request.CommissionRate;
                
                if (string.IsNullOrWhiteSpace(storeName))
                {
                    await trans.RollbackAsync(cancellationToken);
                    return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
                }
                
                if (commissionRate <= 0 || commissionRate >= 100)
                {
                    await trans.RollbackAsync(cancellationToken);
                    return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
                }
            }

            // Role is ALWAYS Vendor (server-determined, never from request)
            const string targetRole = Roles.Merchant;

            // Validate creator can create Vendor role
            var roleValidation = await userCreationService.ValidateRoleAssignmentAsync(creatorId, targetRole);
            if (!roleValidation.Succeeded)
            {
                await trans.RollbackAsync(cancellationToken);
                return roleValidation;
            }

            // Create user with Vendor role and default claims
            var createResult = await userCreationService.CreateUserWithRoleAsync(
                appUser,
                request.Password!,
                targetRole,
                creatorId);

            if (!createResult.Succeeded)
            {
                await trans.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());
            }

            // Create Vendor profile
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

            await dbContext.Vendors.AddAsync(vendor, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await notificationService.CreateAsync(
                "merchant_registered",
                new { merchantId = appUser.Id, storeName, commissionRate },
                new NotificationRecipients(
                    AdminIds: creatorRole == Roles.Admin ? new[] { creatorId } : null,
                    MerchantIds: new[] { appUser.Id }),
                cancellationToken);

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
