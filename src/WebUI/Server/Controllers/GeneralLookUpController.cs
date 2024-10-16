using CheetahExam.Application.GeneralLookups.Commands.Create;
using CheetahExam.Application.GeneralLookups.Commands.Delete;
using CheetahExam.Application.GeneralLookups.Commands.Retrieve;
using CheetahExam.Application.GeneralLookups.Commands.Update;
using CheetahExam.Application.GeneralLookups.Queries.GetGeneralLookUpsWithPagination;
using CheetahExam.Application.GeneralLookups.Queries.GetGeneralLoopUpsWithType;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CheetahExam.WebUI.Server.Controllers;

[Authorize(Roles = $"{Constant.UserRoles.SuperAdmin},{Constant.UserRoles.Admin}")]
[Route("/")]
public class GeneralLookUpsController : ApiControllerBase
{
    [HttpGet("generalLookUps/getAll")]
    [OpenApiOperation("List all general lookups  types", "")]
    public async Task<List<GeneralLookUpDto>> GetAll(bool isIncludeArchived)
    {
        return await Mediator.Send(new GetGeneralLookUpsWithPaginationQuery { PageNumber = 1, PageSize = 200, ISIncludeArchived = isIncludeArchived });
    }

    [HttpPost("generalLookUps/create")]
    public async Task<Result> Create(GeneralLookUpDto generalLookUp)
    {
        return await Mediator.Send(new CreateGeneralLookUpCommand { GeneralLookUp = generalLookUp });
    }

    [HttpPut("generalLookUps/update")]
    public async Task<Result> Update(GeneralLookUpDto generalLookUp)
    {
        return await Mediator.Send(new UpdateGeneralLookUpCommand { GeneralLookUp = generalLookUp });
    }

    [HttpPost("generalLookUps/delete")]
    public async Task<Result> Delete(string uniqueId)
    {
        return await Mediator.Send(new DeleteGeneralLookUpCommand { UniqueId = uniqueId });
    }

    [HttpPost("generalLookUps/retrieve")]
    public async Task<Result> Retrieve(string uniqueId)
    {
        return await Mediator.Send(new RetrieveGeneralLoopUpCommand { UniqueId = uniqueId });
    }

    [HttpGet("generalLookUps/getByType")]
    public async Task<KeyValue[]> GetByType(string generalLookUpType)
    {
        return await Mediator.Send(new GetGeneralLoopUpsWithTypeQuery { GeneralLookUpType = generalLookUpType });
    }
}
