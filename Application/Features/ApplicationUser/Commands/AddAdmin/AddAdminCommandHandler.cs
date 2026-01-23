using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Authorization;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ApplicationUser.Commands.AddAdmin;

internal class AddAdminCommandHandler(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IUserCreationService userCreationService,
    IFileUploadService fileUploadService,
    INotificationService notificationService) : ApiResponseHandler(),
    IRequestHandler<AddAdminCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        var fullName = $"{request.FirstName} {request.LastName}".Trim();
        var appUser = new AppUser(request.UserName, fullName)
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true // Admin users are created by other admins, so email is trusted
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

            // Get creator ID and validate permissions
            var creatorId = currentUserService.GetUserId();
            
            // Determine target role based on creator (server-side only)
            // SuperAdmin can create SuperAdmin or Admin
            // Admin can only create Admin (NOT SuperAdmin)
            var creatorRoles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(creatorId.ToString())!);
            var creatorRole = creatorRoles.FirstOrDefault();
            
            string targetRole = Roles.Admin; // Default to Admin
            if (creatorRole == Roles.SuperAdmin)
            {
                // For now, SuperAdmin creates Admin. Can be extended to allow SuperAdmin creation
                targetRole = Roles.Admin;
            }

            // Validate creator can create this role
            var roleValidation = await userCreationService.ValidateRoleAssignmentAsync(creatorId, targetRole);
            if (!roleValidation.Succeeded)
            {
                await trans.RollbackAsync(cancellationToken);
                return roleValidation;
            }

            // Create user with role and default claims
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

            // Create Admin profile
            var admin = new Admin(
                appUserId: appUser.Id,
                fullName: fullName,
                gender: request.Gender ?? Gender.Unspecified,
                address: request.Address ?? string.Empty,
                createdBy: creatorId
            );

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                admin.ChangePhoneNumber(request.PhoneNumber, creatorId);
            }

            if (!string.IsNullOrWhiteSpace(request.SecondPhoneNumber))
            {
                admin.ChangeSecondPhoneNumber(request.SecondPhoneNumber, creatorId);
            }

            await dbContext.Admins.AddAsync(admin, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await notificationService.CreateAsync(
                "new_user",
                new { userId = appUser.Id, userName = appUser.UserName, role = targetRole },
                new NotificationRecipients(
                    AdminIds: creatorRole == Roles.Admin ? new[] { creatorId } : null),
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
