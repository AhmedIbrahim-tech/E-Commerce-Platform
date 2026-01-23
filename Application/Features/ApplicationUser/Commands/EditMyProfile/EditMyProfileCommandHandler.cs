using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Domain.Entities.AuditLogs;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using System.Text.Json;
using Infrastructure.Data.Identity;

namespace Application.Features.ApplicationUser.Commands.EditMyProfile;

public class EditMyProfileCommandHandler(
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<EditMyProfileCommand, ApiResponse<EditMyProfileResponse>>
{
    public async Task<ApiResponse<EditMyProfileResponse>> Handle(EditMyProfileCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated)
        {
            return new ApiResponse<EditMyProfileResponse>(UserErrors.InvalidCredentials());
        }

        var userId = currentUserService.GetUserId();
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return new ApiResponse<EditMyProfileResponse>(UserErrors.UserNotFound());
        }

        var changedFields = new List<string>();

        if (!string.IsNullOrWhiteSpace(request.DisplayName) && request.DisplayName != user.DisplayName)
        {
            user.SetDisplayName(request.DisplayName);
            changedFields.Add("displayName");
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && request.PhoneNumber != user.PhoneNumber)
        {
            user.PhoneNumber = request.PhoneNumber;
            changedFields.Add("phoneNumber");
        }

        if (request.ProfileImage != null)
        {
            await fileUploadService.TryDeleteFileAsync(user.ProfileImage, cancellationToken);

            var profileImageUrls = await fileUploadService.UploadAndGetUrlsAsync(
                new[] { request.ProfileImage },
                FileLocations.Users,
                user.Id,
                childFolder: null,
                overwrite: true,
                cancellationToken: cancellationToken);

            if (profileImageUrls.Count > 0 && !string.IsNullOrWhiteSpace(profileImageUrls[0]))
            {
                user.ProfileImage = profileImageUrls[0];
                changedFields.Add("profileImage");
            }
        }

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return BadRequest<EditMyProfileResponse>("Failed to update profile");
        }

        var response = new EditMyProfileResponse
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            DisplayName = user.DisplayName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            ProfileImageUrl = fileUploadService.ToAbsoluteUrl(user.ProfileImage)
        };

        unitOfWork.Context.AuditLogs.Add(new AuditLog(
            eventType: "Profile",
            eventName: "ProfileUpdated",
            description: "Profile updated",
            userId: user.Id,
            userEmail: user.Email,
            additionalData: JsonSerializer.Serialize(new
            {
                entityType = "profile",
                entityId = user.Id,
                fields = changedFields
            })));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Edit(response, "Profile updated successfully");
    }
}

