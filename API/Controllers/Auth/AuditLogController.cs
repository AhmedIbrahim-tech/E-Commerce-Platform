using Application.Features.AuditLogs.Queries.GetAuditLogById;
using Application.Features.AuditLogs.Queries.GetAuditLogPaginatedList;
using API.Controllers.Base;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Auth
{
    [Authorize(Roles = Roles.Admin)]
    public class AuditLogController : AppControllerBase
    {
        [Authorize(Policy = Policies.Admin.ViewList)]
        [HttpPost(Router.AuditLogRouting.Paginated)]
        public async Task<IActionResult> GetAuditLogPaginatedList([FromBody] GetAuditLogPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = Policies.Admin.ViewProfile)]
        [HttpGet(Router.AuditLogRouting.GetById)]
        public async Task<IActionResult> GetAuditLogById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetAuditLogByIdQuery(id)));
        }
    }
}
