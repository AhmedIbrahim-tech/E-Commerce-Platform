using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Services;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Vendors.Queries.GetVendorById;

public class GetVendorByIdQueryHandler : ApiResponseHandler,
    IRequestHandler<GetVendorByIdQuery, ApiResponse<GetVendorByIdResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileUploadService _fileUploadService;

    public GetVendorByIdQueryHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IFileUploadService fileUploadService) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _fileUploadService = fileUploadService;
    }

    public async Task<ApiResponse<GetVendorByIdResponse>> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _dbContext.Vendors
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<GetVendorByIdResponse>(VendorErrors.VendorNotFound());

        var appUser = await _userManager.FindByIdAsync(vendor.AppUserId.ToString());
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
            ProfileImage = _fileUploadService.ToAbsoluteUrl(appUser.ProfileImage)
        };

        return Success(vendorResponse);
    }
}
