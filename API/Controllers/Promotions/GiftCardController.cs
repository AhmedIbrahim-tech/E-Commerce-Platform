using Application.Features.GiftCards.Commands.AddGiftCard;
using Application.Features.GiftCards.Commands.EditGiftCard;
using Application.Features.GiftCards.Commands.DeleteGiftCard;
using Application.Features.GiftCards.Queries.GetGiftCardList;
using Application.Features.GiftCards.Queries.GetGiftCardById;
using Application.Features.GiftCards.Queries.GetGiftCardPaginatedList;
using API.Controllers.Base;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Promotions
{
    [Authorize]
    public class GiftCardController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.GiftCardRouting.GetAll)]
        public async Task<IActionResult> GetGiftCardList()
        {
            var response = await Mediator.Send(new GetGiftCardListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.GiftCardRouting.Paginated)]
        public async Task<IActionResult> GetGiftCardPaginatedList([FromBody] GetGiftCardPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.GiftCardRouting.Prefix + "getById")]
        public async Task<IActionResult> GetGiftCardById([FromBody] GetGiftCardByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.GiftCard.Create)]
        [HttpPost(Router.GiftCardRouting.Create)]
        public async Task<IActionResult> CreateGiftCard([FromBody] AddGiftCardCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.GiftCard.Edit)]
        [HttpPost(Router.GiftCardRouting.Edit)]
        public async Task<IActionResult> EditGiftCard([FromBody] EditGiftCardCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.GiftCard.Delete)]
        [HttpPost(Router.GiftCardRouting.Prefix + "delete")]
        public async Task<IActionResult> DeleteGiftCard([FromBody] DeleteGiftCardCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
