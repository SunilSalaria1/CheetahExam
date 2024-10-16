using CheetahExam.Application.Options.Commands.Delete;
using CheetahExam.Application.Options.Commands.UpdateOptionsDisplayOrder;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using CheetahExam.WebUI.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CheetahExam.WebUI.Server.Controllers.Exams;

[Route("/")]
[Authorize(Roles = $"{Constant.UserRoles.SuperAdmin},{Constant.UserRoles.Admin}")]
public class OptionsController : ApiControllerBase
{
    [HttpPut("options/update/displayOrders/{questionId}")]
    [OpenApiOperation("Update Options Display orders", "Update the Option Display orders with questionId.")]
    public async Task<bool> UpdateDisplayOrders(string questionId, [FromBody] List<DisplayOrderDto> displayOrders)
    {
        return await Mediator.Send(new UpdateOptionsDisplayOrderCommand() { QuestionId = questionId, OptionDisplayOrders = displayOrders });
    }

    [HttpDelete("options/delete/{optionId}")]
    [OpenApiOperation("Delete a Options", "Remove an exisiting Option with Id.")]
    public async Task<Unit> Delete(string optionId)
    {
        return await Mediator.Send(new DeleteOptionCommand() { OptionId = optionId });
    }
}
