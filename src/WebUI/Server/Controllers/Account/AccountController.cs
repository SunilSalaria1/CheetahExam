using CheetahExam.Application.Accounts.Commands.AuthenticateAccount;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheetahExam.WebUI.Server.Controllers.Account;

[AllowAnonymous]
[Route("/")]
public class AccountController : ApiControllerBase
{
    [HttpPost("account/login")]
    public async Task<ResultDto> Login([FromBody] LoginDto login)
    {
        return await Mediator.Send(new AuthenticateAccountCommand() { Login = login });
    }

    /// <summary>
    /// this end point is used for providing username using direct api call to GetAuthenticationStateAsync() method
    /// </summary>
    /// <returns></returns>

    [HttpGet("account/name")]
    public IActionResult GetCurrentUser()
    {
        var currentUser = User.Identity.IsAuthenticated ? User.Identity.Name : null;
        return Ok(currentUser);
    }

    // This action method is for registering users from registration page which is currently removed. 

    //[HttpPost("account/register")]
    //public async Task<ResultDto> Register([FromBody] RegisterDto user)
    //{
    //    return user.Password != user.ConfirmPassword ? (ResultDto)new() { Errors = } : await Mediator.Send(new CreateAccountCommand() { User = user });
    //}
}
