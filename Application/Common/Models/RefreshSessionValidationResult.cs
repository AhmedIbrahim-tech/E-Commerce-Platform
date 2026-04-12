namespace Application.Common.Models;

/// <summary>
/// Result of validating a refresh-token session (DB + optional access-token binding).
/// </summary>
public sealed record RefreshSessionValidationResult(
    bool Success,
    string? ErrorCode,
    Guid UserId,
    DateTimeOffset RefreshExpiresAt,
    string OldJwtId);
