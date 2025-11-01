using Core.Features.Reviews.Commands.Models;
using Core.Features.Reviews.Queries.Models;

namespace API.Controllers
{
    [Authorize]
    public class ReviewController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.ReviewRouting.Paginated)]
        public async Task<IActionResult> GetReviewPaginatedList([FromQuery] GetReviewPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPost(Router.ReviewRouting.Create)]
        public async Task<IActionResult> CreateReview([FromBody] AddReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.ReviewRouting.Edit)]
        public async Task<IActionResult> EditReview([FromBody] EditReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ReviewRouting.Delete)]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteReviewCommand(id)));
        }
    }
}
