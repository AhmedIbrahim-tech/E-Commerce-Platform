using Application.Features.Deliveries.Commands.SetDeliveryMethod;
using Application.Features.Deliveries.Commands.EditDeliveryMethod;
using API.Controllers.Base;

namespace API.Controllers
{
    [Authorize(Roles = "Customer")]
    public class DeliveryController : AppControllerBase
    {
        [HttpPost(Router.DeliveryRouting.SetDeliveryMethod)]
        public async Task<IActionResult> SetDeliveryMethod([FromBody] SetDeliveryMethodCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpPut(Router.DeliveryRouting.EditDeliveryMethod)]
        public async Task<IActionResult> EditDeliveryMethod([FromBody] EditDeliveryMethodCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
