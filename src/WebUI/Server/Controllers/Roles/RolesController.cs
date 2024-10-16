using CheetahExam.Application.Roles.Query.GetRolesWithCompanyId;
using CheetahExam.Application.Roles.Query.GetRoleWithUserId;
using CheetahExam.WebUI.Shared.Common.Models.Users;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheetahExam.WebUI.Server.Controllers.Roles;

[Authorize(Roles = $"{Constant.UserRoles.Administrator},{Constant.UserRoles.SuperAdmin}")]
[Route("/")]
public class RolesController : ApiControllerBase
{
    [HttpGet("roles/getAll/{companyId}")]
    public async Task<RolesDto> GetAllRolesByCompanyId(string companyId)
    {
        return await Mediator.Send(new GetRolesWithCompanyIdQuery() { CompanyId = companyId });
    }

    [HttpGet("roles/get/{userId}")]
    public async Task<RoleDto> GetRolesByUserId(string userId)
    {
        return await Mediator.Send(new GetRoleWithUserIdQuery() { UserId = userId });
    }
}
