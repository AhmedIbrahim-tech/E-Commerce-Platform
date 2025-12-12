using Application.Features.Payments.Commands.SetPaymentMethod;
using Application.Features.Payments.Commands.ServerCallback;
using Application.Features.Payments.Queries.PaymobCallback;
using API.Controllers.Base;

namespace API.Controllers
{
    [Authorize]
    public class PaymentController : AppControllerBase
    {
        [Authorize(Roles = "Customer")]
        [HttpPost(Router.PaymentRouting.SetPaymentMethod)]
        public async Task<IActionResult> SetPaymentMethod([FromBody] SetPaymentMethodCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPost(Router.PaymentRouting.ServerCallback)]
        public async Task<IActionResult> ServerCallback([FromBody] JsonElement payload)
        {
            var hmac = Request.Query["hmac"].ToString();

            return Ok(await Mediator.Send(new ServerCallbackCommand(payload, hmac)));
        }

        [AllowAnonymous]
        [HttpGet(Router.PaymentRouting.PaymobCallback)]
        public async Task<IActionResult> PaymobCallback(PaymobCallbackQuery query)
        {
            var hmac = Request.Query["hmac"].ToString();

            return Ok(await Mediator.Send(query));
        }
    }
}
