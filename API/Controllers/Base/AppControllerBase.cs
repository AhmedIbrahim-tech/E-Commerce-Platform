using Application.Common.Bases;

namespace API.Controllers.Base;

[ApiController]
public class AppControllerBase : ControllerBase
{
    private IMediator? _mediator;
    
    protected IMediator Mediator => 
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    public IActionResult NewResult<T>(ApiResponse<T> response)
    {
        if (!response.Succeeded)
        {
            return response.ToProblem();
        }

        return CreateSuccessResult(response);
    }

    private static IActionResult CreateSuccessResult<T>(ApiResponse<T> response)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.OK => new OkObjectResult(response),
            HttpStatusCode.Created => new CreatedResult(string.Empty, response),
            HttpStatusCode.Accepted => new AcceptedResult(string.Empty, response),
            _ => new OkObjectResult(response)
        };
    }
}
