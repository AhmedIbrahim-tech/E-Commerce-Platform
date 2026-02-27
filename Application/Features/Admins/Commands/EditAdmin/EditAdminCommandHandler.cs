using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Services;
using Domain.Entities.Users;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Admins.Commands.EditAdmin;

public class EditAdminCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<EditAdminCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await unitOfWork.Admins.GetTableAsTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (admin is null) return new ApiResponse<string>(AdminErrors.AdminNotFound());

        var appUser = await userManager.FindByIdAsync(admin.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var isUserNameDuplicate = await userManager.UserNameExistsAsync(request.UserName!, admin.AppUserId);
        if (isUserNameDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        var isEmailDuplicate = await userManager.EmailExistsAsync(request.Email!, admin.AppUserId);
        if (isEmailDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        appUser.UserName = request.UserName;
        appUser.Email = request.Email;
        appUser.PhoneNumber = request.PhoneNumber;
        var fullName = $"{request.FirstName} {request.LastName}".Trim();
        appUser.SetDisplayName(fullName);

        if (request.ProfileImage != null)
        {
            await fileUploadService.TryDeleteFileAsync(appUser.ProfileImage, cancellationToken);
            var profileImageUrls = await fileUploadService.UploadAndGetUrlsAsync(
                new[] { request.ProfileImage },
                Application.Common.Constants.FileLocations.Users,
                appUser.Id,
                childFolder: null,
                overwrite: true,
                cancellationToken: cancellationToken);

            if (profileImageUrls.Count > 0)
                appUser.ProfileImage = profileImageUrls[0];
        }

        admin.ChangeName(fullName, admin.AppUserId);
        if (request.Gender.HasValue)
        {
            admin.ChangeGender(request.Gender.Value, admin.AppUserId);
        }

        if (!string.IsNullOrWhiteSpace(request.Address))
        {
            admin.ChangeAddress(request.Address, admin.AppUserId);
        }

        admin.ChangePhoneNumber(request.PhoneNumber, admin.AppUserId);
        admin.ChangeSecondPhoneNumber(request.SecondPhoneNumber, admin.AppUserId);

        var updateAppUserResult = await userManager.UpdateAsync(appUser);
        if (!updateAppUserResult.Succeeded)
            return new ApiResponse<string>(AdminErrors.InvalidAdminData());

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Edit("");
    }
}
