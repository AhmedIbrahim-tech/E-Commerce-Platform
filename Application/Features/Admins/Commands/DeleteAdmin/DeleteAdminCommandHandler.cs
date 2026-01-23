using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Admins.Commands.DeleteAdmin;

public class DeleteAdminCommandHandler : ApiResponseHandler,
    IRequestHandler<DeleteAdminCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public DeleteAdminCommandHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<string>> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await _dbContext.Admins
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (admin is null) return new ApiResponse<string>(AdminErrors.AdminNotFound());

        var appUser = await _userManager.FindByIdAsync(admin.AppUserId.ToString());
        if (appUser != null)
        {
            var deleteResult = await _userManager.DeleteAsync(appUser);
            if (!deleteResult.Succeeded)
                return new ApiResponse<string>(AdminErrors.CannotDeleteAdmin());
        }

        admin.MarkDeleted(admin.AppUserId);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Deleted<string>();
    }
}
