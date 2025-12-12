using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Emails.Commands.SendEmail;

public class SendEmailCommandHandler(IEmailService emailService) : ApiResponseHandler(),
    IRequestHandler<SendEmailCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var response = await emailService.SendEmailAsync(request.Email, request.ReturnUrl, request.EmailType, null);
        if (response == "Success")
            return Success("");
        return new ApiResponse<string>(EmailErrors.EmailSendFailed());
    }
}

