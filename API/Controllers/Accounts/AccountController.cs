using Application.Features.Accounts.Commands.AddAccount;
using Application.Features.Accounts.Commands.EditAccount;
using Application.Features.Accounts.Commands.DeleteAccount;
using Application.Features.Accounts.Queries.GetAccountList;
using Application.Features.Accounts.Queries.GetAccountById;
using Application.Features.Accounts.Queries.GetAccountPaginatedList;
using API.Controllers.Base;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Accounts
{
    [Authorize]
    public class AccountController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.AccountRouting.GetAll)]
        public async Task<IActionResult> GetAccountList()
        {
            var response = await Mediator.Send(new GetAccountListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.AccountRouting.Paginated)]
        public async Task<IActionResult> GetAccountPaginatedList([FromBody] GetAccountPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.AccountRouting.Prefix + "getById")]
        public async Task<IActionResult> GetAccountById([FromBody] GetAccountByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Account.Create)]
        [HttpPost(Router.AccountRouting.Create)]
        public async Task<IActionResult> CreateAccount([FromBody] AddAccountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Account.Edit)]
        [HttpPost(Router.AccountRouting.Edit)]
        public async Task<IActionResult> EditAccount([FromBody] EditAccountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Account.Delete)]
        [HttpPost(Router.AccountRouting.Prefix + "delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
