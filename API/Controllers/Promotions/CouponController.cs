using Application.Features.Coupons.Commands.AddCoupon;
using Application.Features.Coupons.Commands.DeleteCoupon;
using Application.Features.Coupons.Commands.EditCoupon;
using Application.Features.Coupons.Queries.GetCouponById;
using Application.Features.Coupons.Queries.GetCouponList;
using Application.Features.Coupons.Queries.GetCouponPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Promotions;

[Authorize]
public class CouponController : AppControllerBase
{
    [AllowAnonymous]
    [HttpPost(Router.CouponRouting.GetAll)]
    public async Task<IActionResult> GetCouponList()
    {
        var response = await Mediator.Send(new GetCouponListQuery());
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.CouponRouting.Paginated)]
    public async Task<IActionResult> GetCouponPaginatedList([FromBody] GetCouponPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.CouponRouting.Prefix + "getById")]
    public async Task<IActionResult> GetCouponById([FromBody] GetCouponByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Coupon.Create)]
    [HttpPost(Router.CouponRouting.Create)]
    public async Task<IActionResult> CreateCoupon([FromBody] AddCouponCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Coupon.Edit)]
    [HttpPost(Router.CouponRouting.Edit)]
    public async Task<IActionResult> EditCoupon([FromBody] EditCouponCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Coupon.Delete)]
    [HttpPost(Router.CouponRouting.Prefix + "delete")]
    public async Task<IActionResult> DeleteCoupon([FromBody] DeleteCouponCommand command)
    {
        return NewResult(await Mediator.Send(command));
    }
}
