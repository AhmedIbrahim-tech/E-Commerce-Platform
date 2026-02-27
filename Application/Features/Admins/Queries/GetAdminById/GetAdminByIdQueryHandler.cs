using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Admins.Queries.GetAdminById;

public class GetAdminByIdQueryHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetAdminByIdQuery, ApiResponse<GetAdminByIdResponse>>
{
    public async Task<ApiResponse<GetAdminByIdResponse>> Handle(GetAdminByIdQuery request, CancellationToken cancellationToken)
    {
        var admin = await unitOfWork.Admins.GetTableNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (admin is null) return new ApiResponse<GetAdminByIdResponse>(AdminErrors.AdminNotFound());

        var appUser = await userManager.FindByIdAsync(admin.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<GetAdminByIdResponse>(UserErrors.UserNotFound());

        var adminResponse = new GetAdminByIdResponse
        {
            Id = admin.Id,
            AppUserId = admin.AppUserId,
            FullName = admin.FullName ?? string.Empty,
            UserName = appUser.UserName ?? string.Empty,
            Email = appUser.Email ?? string.Empty,
            PhoneNumber = appUser.PhoneNumber ?? string.Empty,
            Gender = admin.Gender,
            Address = admin.Address ?? string.Empty,
            ProfileImage = fileUploadService.ToAbsoluteUrl(appUser.ProfileImage)
        };

        return Success(adminResponse);
    }
}
