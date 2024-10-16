using CheetahExam.Application.Common.Exceptions;
using CheetahExam.Application.Users.Command.Create;
using CheetahExam.Application.Users.Command.Update;
using CheetahExam.Application.Users.Queries;
using CheetahExam.Application.Users.Query.GetCompanyUsersWithId;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Users;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheetahExam.WebUI.Server.Controllers.Users;

[Authorize(Roles = $"{Constant.UserRoles.Administrator},{Constant.UserRoles.SuperAdmin}")]
[Route("/")]
public class UsersController : ApiControllerBase
{
    [HttpPost("users/getAll/{companyId}")]
    public async Task<UsersDto> GetAllUsersByCompanyIdAndRoles(string companyId, List<string> role)
    {
        return await Mediator.Send(new GetCompanyUsersWithIdQuery() { CompanyId = companyId, Roles = role });
    }

    [HttpPost("users/create")]
    public async Task<Result> CreateUser(string companyId, UserDto user)
    {
        return await Mediator.Send(new CreateUserCommand() { CompanyId = companyId, User = user });
    }

    [HttpPut("users/update/{id}")]
    public async Task<Result> UpdateUser(string id, string companyId, UserDto updatedUser)
    {
        return id != updatedUser.UniqueId
            ? Result.Failure(new List<string>() { "Bad Request" })
            : await Mediator.Send(new UpdateUserCommand() { CompanyId = companyId, User = updatedUser });
    }

    [HttpGet("users/get/{id}")]
    public async Task<UserDto> GetUser(string id)
    {
        return await Mediator.Send(new GetUserQuery() { UserId = id });
    }

    [HttpPut("users/updateRole")]
    public async Task<Result> UpdateUserRole(string userId, List<string> roleNames)
    {
        return await Mediator.Send(new UpdateUserRoleCommand() { UserId = userId, RoleNames = roleNames });
    }
}
