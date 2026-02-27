using Application.Common.Bases;

namespace Application.Features.Authentication.TwoStepVerification;

public record TwoStepVerificationQuery(string Code, string? Email) : IRequest<ApiResponse<string>>;
