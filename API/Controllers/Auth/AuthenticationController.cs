using Application.Features.ApplicationUser.Commands.AddCustomer;
using Application.Features.Authentication.AuthorizeUser;
using Application.Features.Authentication.Commands.ChangePassword;
using Application.Features.Authentication.Commands.GoogleLogin;
using Application.Features.Authentication.Commands.Logout;
using Application.Features.Authentication.Commands.LogoutAll;
using Application.Features.Authentication.Commands.RefreshToken;
using Application.Features.Authentication.Commands.ResetPassword;
using Application.Features.Authentication.Commands.SendResetPassword;
using Application.Features.Authentication.Commands.SignIn;
using Application.Features.Authentication.ConfirmEmail;
using Application.Features.Authentication.ConfirmResetPassword;
using Application.Features.Authentication.TwoStepVerification;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Auth;

[Authorize]
public class AuthenticationController : AppControllerBase
{
    [AllowAnonymous]
    [HttpPost(Router.Authentication.SignIn)]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.SignInViaGoogle)]
    public async Task<IActionResult> SignInViaGoogle([FromBody] GoogleLoginCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.RefreshToken)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [HttpPost(Router.Authentication.ValidateToken)]
    public async Task<IActionResult> ValidateToken([FromBody] AuthorizeUserQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.SendResetPasswordCode)]
    public async Task<IActionResult> SendResetPasswordCode([FromBody] SendResetPasswordCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.ConfirmResetPasswordCode)]
    public async Task<IActionResult> ConfirmResetPasswordCode([FromBody] ConfirmResetPasswordQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.ResetPassword)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.TwoStepVerification)]
    public async Task<IActionResult> TwoStepVerification([FromBody] TwoStepVerificationQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.Authentication.Register)]
    public async Task<IActionResult> Register([FromBody] AddCustomerCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Auth.ChangePassword)]
    [HttpPut(Router.Authentication.ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [HttpPost(Router.Authentication.Logout)]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [HttpPost(Router.Authentication.LogoutAll)]
    public async Task<IActionResult> LogoutAll([FromBody] LogoutAllCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }
}
