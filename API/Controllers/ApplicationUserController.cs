using Core.Features.ApplicationUser.Commands.Models;

namespace API.Controllers
{
    [Authorize]
    public class ApplicationUserController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.UserRouting.Register)]
        public async Task<IActionResult> Register([FromBody] AddCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.UserRouting.ChangePassword)]
        public async Task<IActionResult> ChangePasword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
