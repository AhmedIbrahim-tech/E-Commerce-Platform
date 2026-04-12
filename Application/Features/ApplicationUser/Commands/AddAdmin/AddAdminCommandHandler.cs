using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Domain.Entities.Users;
using Infrastructure.Data.Authorization;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ApplicationUser.Commands.AddAdmin;

internal class AddAdminCommandHandler(
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IUserCreationService userCreationService,
    IFileUploadService fileUploadService,
    INotificationService notificationService) : ApiResponseHandler(),
    IRequestHandler<AddAdminCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        var fullName = request.FullName?.Trim() ?? string.Empty;
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
            
            var creatorRoles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(creatorId.ToString())!);
            var creatorRole = creatorRoles.FirstOrDefault();
            
            string targetRole = Roles.Admin;
            if (creatorRole == Roles.SuperAdmin)
            {
                targetRole = Roles.Admin;
            }

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

            await unitOfWork.Admins.AddAsync(admin, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await notificationService.CreateAsync(
                "new_user",
                new { userId = appUser.Id, userName = appUser.UserName, role = targetRole },
                new NotificationRecipients(
                    AdminIds: creatorRole == Roles.Admin ? new[] { creatorId } : null),
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
