using API.Controllers.Base;
using Application.Features.Emails.Commands.SendEmail;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class EmailsController : AppControllerBase
    {
        [HttpPost(Router.EmailsRoute.SendEmail)]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
