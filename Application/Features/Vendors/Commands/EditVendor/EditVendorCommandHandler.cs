using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Vendors.Commands.EditVendor;

public class EditVendorCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<EditVendorCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await unitOfWork.Vendors.GetTableAsTracking()
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<string>(VendorErrors.VendorNotFound());

        var appUser = await userManager.FindByIdAsync(vendor.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var isUserNameDuplicate = await userManager.UserNameExistsAsync(request.UserName!, vendor.AppUserId);
        if (isUserNameDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        var isEmailDuplicate = await userManager.EmailExistsAsync(request.Email!, vendor.AppUserId);
        if (isEmailDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        if (request.CommissionRate <= 0 || request.CommissionRate >= 100)
            return new ApiResponse<string>(VendorErrors.InvalidCommissionRate());

        appUser.UserName = request.UserName;
        appUser.Email = request.Email;
        appUser.PhoneNumber = request.PhoneNumber;
        var fullName = request.FullName?.Trim() ?? string.Empty;
        appUser.SetDisplayName(fullName);

        if (request.ProfileImage != null)
        {
            await fileUploadService.TryDeleteFileAsync(appUser.ProfileImage, cancellationToken);
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

        vendor.ChangeName(fullName, vendor.AppUserId);
        
        if (!string.IsNullOrWhiteSpace(request.StoreName))
        {
            vendor.UpdateStoreName(request.StoreName, vendor.AppUserId);
        }

        vendor.UpdateCommissionRate(request.CommissionRate, vendor.AppUserId);

        vendor.ChangePhoneNumber(request.PhoneNumber, vendor.AppUserId);
        vendor.ChangeSecondPhoneNumber(request.SecondPhoneNumber, vendor.AppUserId);

        var updateAppUserResult = await userManager.UpdateAsync(appUser);
        if (!updateAppUserResult.Succeeded)
            return new ApiResponse<string>(VendorErrors.InvalidVendorData());

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Edit("");
    }
}
