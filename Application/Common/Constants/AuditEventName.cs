namespace Application.Common.Constants;

public static class AuditEventName
{
    public const string UserLogin = "UserLogin";
    public const string UserLogout = "UserLogout";
    public const string UserLogoutAll = "UserLogoutAll";
    public const string LoginAttemptFailed = "LoginAttemptFailed";
    public const string AccountLocked = "AccountLocked";
    public const string RefreshTokenBlocked = "RefreshTokenBlocked";
    public const string RefreshTokenNotFound = "RefreshTokenNotFound";
    public const string RefreshTokenReused = "RefreshTokenReused";
    public const string RevokedRefreshTokenUsed = "RevokedRefreshTokenUsed";
    public const string RefreshTokenExpired = "RefreshTokenExpired";
    public const string RefreshTokenRotated = "RefreshTokenRotated";
    public const string InvalidLogoutAttempt = "InvalidLogoutAttempt";
    public const string LogoutError = "LogoutError";
    public const string LogoutAllError = "LogoutAllError";
}
