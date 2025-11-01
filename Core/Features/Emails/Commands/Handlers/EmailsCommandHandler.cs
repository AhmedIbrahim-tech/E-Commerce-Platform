using Core.Features.Emails.Commands.Models;

namespace Core.Features.Emails.Commands.Handlers
{
    public class EmailsCommandHandler : ApiResponseHandler,
        IRequestHandler<SendEmailCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IEmailsService _emailsService;
        #endregion

        #region Constructors
        public EmailsCommandHandler(IEmailsService emailsService) : base()
        {
            _emailsService = emailsService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailsService.SendEmailAsync(request.Email, request.ReturnUrl, request.EmailType);
            if (response == "Success")
                return Success("");
            return BadRequest<string>("SendEmailFailed");
        }
        #endregion
    }
}
