using Application.Common.Bases;
using Application.Common.Errors;
using Application.Common.Helpers;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IAuthenticationService authenticationService,
    IUnitOfWork unitOfWork,
    IAuditService auditService) : ApiResponseHandler(), 
    IRequestHandler<SignInCommand, ApiResponse<JwtAuthResponse>>
{
    public async Task<ApiResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Login attempt with non-existent email",
                userEmail: request.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());
        }

        if (!user.EmailConfirmed)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Login attempt with unconfirmed email",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.EmailNotConfirmed());
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        
        if (signInResult.IsLockedOut)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "AccountLocked",
                description: "Login attempt on locked account",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "AccountLocked",
                description: "Login attempt on locked account (manual lockout)",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (!signInResult.Succeeded)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Invalid password provided",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());
        }

        var isDeleted = await (from c in unitOfWork.Customers.GetTableNoTracking()
                                where c.AppUserId == user.Id && c.IsDeleted
                                select true)
                                .Union(from v in unitOfWork.Vendors.GetTableNoTracking()
                                       where v.AppUserId == user.Id && v.IsDeleted
                                       select true)
                                .Union(from a in unitOfWork.Admins.GetTableNoTracking()
                                       where a.AppUserId == user.Id && a.IsDeleted
                                       select true)
                                .AnyAsync(cancellationToken);

        if (isDeleted)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Login attempt on disabled/deleted account",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.DisabledUser());
        }

        var result = await authenticationService.GetJWTTokenAsync(user);
        
        _ = auditService.LogEventAsync(
            eventType: "Authentication",
            eventName: "UserLogin",
            description: "User logged in successfully",
            userId: user.Id,
            userEmail: user.Email,
            cancellationToken: cancellationToken
        );
        
        return Success(result);
    }
}

