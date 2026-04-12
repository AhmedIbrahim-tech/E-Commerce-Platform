using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Customers.Commands.EditCustomer;

public class EditCustomerCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<EditCustomerCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetTableAsTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<string>(CustomerErrors.CustomerNotFound());

        var appUser = await userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var isUserNameDuplicate = await userManager.UserNameExistsAsync(request.UserName!, customer.AppUserId);
        if (isUserNameDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        var isEmailDuplicate = await userManager.EmailExistsAsync(request.Email!, customer.AppUserId);
        if (isEmailDuplicate)
            return new ApiResponse<string>(CustomerErrors.DuplicatedPhoneNumber());

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

        customer.ChangeName(fullName, appUser.Id);
        if (request.Gender.HasValue)
        {
            customer.ChangeGender(request.Gender.Value, appUser.Id);
        }

        customer.ChangePhoneNumber(request.PhoneNumber, appUser.Id);
        customer.ChangeSecondPhoneNumber(request.SecondPhoneNumber, appUser.Id);

        var updateAppUserResult = await userManager.UpdateAsync(appUser);
        if (!updateAppUserResult.Succeeded)
            return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Edit("");
    }
}
