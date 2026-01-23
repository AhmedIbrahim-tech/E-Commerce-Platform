using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Application.ServicesHandlers.Services;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Vendors.Commands.EditVendor;

public class EditVendorCommandHandler : ApiResponseHandler,
    IRequestHandler<EditVendorCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileUploadService _fileUploadService;

    public EditVendorCommandHandler(
        ApplicationDbContext dbContext, 
        UserManager<AppUser> userManager,
        IFileUploadService fileUploadService) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _fileUploadService = fileUploadService;
    }

    public async Task<ApiResponse<string>> Handle(EditVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _dbContext.Vendors
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<string>(VendorErrors.VendorNotFound());

        var appUser = await _userManager.FindByIdAsync(vendor.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var isUserNameDuplicate = await _userManager.UserNameExistsAsync(request.UserName!, vendor.AppUserId);
        if (isUserNameDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        var isEmailDuplicate = await _userManager.EmailExistsAsync(request.Email!, vendor.AppUserId);
        if (isEmailDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        if (request.CommissionRate <= 0 || request.CommissionRate >= 100)
            return new ApiResponse<string>(VendorErrors.InvalidCommissionRate());

        appUser.UserName = request.UserName;
        appUser.Email = request.Email;
        appUser.PhoneNumber = request.PhoneNumber;
        var fullName = $"{request.FirstName} {request.LastName}".Trim();
        appUser.SetDisplayName(fullName);

        if (request.ProfileImage != null)
        {
            await _fileUploadService.TryDeleteFileAsync(appUser.ProfileImage, cancellationToken);
            var profileImageUrls = await _fileUploadService.UploadAndGetUrlsAsync(
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

        var updateAppUserResult = await _userManager.UpdateAsync(appUser);
        if (!updateAppUserResult.Succeeded)
            return new ApiResponse<string>(VendorErrors.InvalidVendorData());

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Edit("");
    }
}
