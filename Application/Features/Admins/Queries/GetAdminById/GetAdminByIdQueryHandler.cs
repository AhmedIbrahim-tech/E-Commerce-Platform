using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Services;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Admins.Queries.GetAdminById;

public class GetAdminByIdQueryHandler : ApiResponseHandler,
    IRequestHandler<GetAdminByIdQuery, ApiResponse<GetAdminByIdResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileUploadService _fileUploadService;

    public GetAdminByIdQueryHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IFileUploadService fileUploadService) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _fileUploadService = fileUploadService;
    }

    public async Task<ApiResponse<GetAdminByIdResponse>> Handle(GetAdminByIdQuery request, CancellationToken cancellationToken)
    {
        var admin = await _dbContext.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (admin is null) return new ApiResponse<GetAdminByIdResponse>(AdminErrors.AdminNotFound());

        var appUser = await _userManager.FindByIdAsync(admin.AppUserId.ToString());
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
            ProfileImage = _fileUploadService.ToAbsoluteUrl(appUser.ProfileImage)
        };

        return Success(adminResponse);
    }
}
