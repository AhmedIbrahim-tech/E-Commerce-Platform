using Application.Common.Errors;

namespace Application.Features.Authentication.TwoStepVerification;

public class TwoStepVerificationQueryHandler : ApiResponseHandler,
    IRequestHandler<TwoStepVerificationQuery, ApiResponse<string>>
{
    public Task<ApiResponse<string>> Handle(TwoStepVerificationQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return Task.FromResult(new ApiResponse<string>(UserErrors.InvalidCode()));
        }

        return Task.FromResult(Success<string>("Verified"));
    }
}
