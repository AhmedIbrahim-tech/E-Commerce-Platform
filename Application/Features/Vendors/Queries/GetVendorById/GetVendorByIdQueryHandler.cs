using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Vendors.Queries.GetVendorById;

public class GetVendorByIdQueryHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetVendorByIdQuery, ApiResponse<GetVendorByIdResponse>>
{
    public async Task<ApiResponse<GetVendorByIdResponse>> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        var vendor = await unitOfWork.Vendors.GetTableNoTracking()
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<GetVendorByIdResponse>(VendorErrors.VendorNotFound());

        var appUser = await userManager.FindByIdAsync(vendor.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<GetVendorByIdResponse>(UserErrors.UserNotFound());

        var vendorResponse = new GetVendorByIdResponse
        {
            Id = vendor.Id,
            FullName = vendor.FullName ?? string.Empty,
            UserName = appUser.UserName ?? string.Empty,
            Email = appUser.Email ?? string.Empty,
            PhoneNumber = appUser.PhoneNumber ?? string.Empty,
            Gender = vendor.Gender,
            StoreName = vendor.StoreName ?? string.Empty,
            CommissionRate = vendor.CommissionRate,
            ProfileImage = fileUploadService.ToAbsoluteUrl(appUser.ProfileImage)
        };

        return Success(vendorResponse);
    }
}
