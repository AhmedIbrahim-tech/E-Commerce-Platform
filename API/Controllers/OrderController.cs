using Application.Features.Orders.Commands.AddOrder;
using Application.Features.Orders.Commands.PlaceOrder;
using Application.Features.Orders.Commands.DeleteOrder;
using Application.Features.Orders.Queries.GetOrderById;
using Application.Features.Orders.Queries.GetMyOrders;
using Application.Features.Orders.Queries.GetOrderPaginatedList;
using API.Controllers.Base;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : AppControllerBase
    {
        [Authorize(Roles = "Customer")]
        [HttpGet(Router.OrderRouting.GetMyOrders)]
        public async Task<IActionResult> GetMyOrders([FromQuery] GetMyOrdersQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet(Router.OrderRouting.GetById)]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id, [FromQuery] int orderPageNumber = 1, [FromQuery] int orderPageSize = 10)
        {
            var response = await Mediator.Send(new GetOrderByIdQuery(id) { OrderPageNumber = orderPageNumber, OrderPageSize = orderPageSize });
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet(Router.OrderRouting.Paginated)]
        public async Task<IActionResult> GetOrderPaginatedList([FromQuery] GetOrderPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost(Router.OrderRouting.Create)]
        public async Task<IActionResult> CreateOrder()
        {
            var response = await Mediator.Send(new AddOrderCommand());
            return Ok(response);
        }

        [Authorize(Roles = "Customer")]
        [HttpPut(Router.OrderRouting.PlaceOrder)]
        public async Task<IActionResult> PlaceOrder([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new PlaceOrderCommand(id));
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete(Router.OrderRouting.Delete)]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new DeleteOrderCommand(id));
            return Ok(response);
        }
    }
}
