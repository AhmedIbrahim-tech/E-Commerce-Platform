using Application.Features.Discounts.Commands.AddDiscount;
using Application.Features.Discounts.Commands.EditDiscount;
using Application.Features.Discounts.Commands.DeleteDiscount;
using Application.Features.Discounts.Queries.GetDiscountList;
using Application.Features.Discounts.Queries.GetDiscountById;
using Application.Features.Discounts.Queries.GetDiscountPaginatedList;
using API.Controllers.Base;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Promotions
{
    [Authorize]
    public class DiscountController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.DiscountRouting.GetAll)]
        public async Task<IActionResult> GetDiscountList()
        {
            var response = await Mediator.Send(new GetDiscountListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.DiscountRouting.Paginated)]
        public async Task<IActionResult> GetDiscountPaginatedList([FromBody] GetDiscountPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.DiscountRouting.Prefix + "getById")]
        public async Task<IActionResult> GetDiscountById([FromBody] GetDiscountByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Discount.Create)]
        [HttpPost(Router.DiscountRouting.Create)]
        public async Task<IActionResult> CreateDiscount([FromBody] AddDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Discount.Edit)]
        [HttpPost(Router.DiscountRouting.Edit)]
        public async Task<IActionResult> EditDiscount([FromBody] EditDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Discount.Delete)]
        [HttpPost(Router.DiscountRouting.Prefix + "delete")]
        public async Task<IActionResult> DeleteDiscount([FromBody] DeleteDiscountCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
