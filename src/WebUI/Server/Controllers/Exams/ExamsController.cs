using CheetahExam.Application.Exams.Commands.Create;
using CheetahExam.Application.Exams.Commands.Delete;
using CheetahExam.Application.Exams.Commands.Export;
using CheetahExam.Application.Exams.Commands.Update;
using CheetahExam.Application.Exams.Queries.GetActiveExamsCount;
using CheetahExam.Application.Exams.Queries.GetAllExamFonts;
using CheetahExam.Application.Exams.Queries.GetAllExams;
using CheetahExam.Application.Exams.Queries.GetExamWithId;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheetahExam.WebUI.Server.Controllers.Exams
{
    [Authorize(Roles = $"{Constant.UserRoles.SuperAdmin},{Constant.UserRoles.Admin}")]
    [Route("/")]
    public class ExamsController : ApiControllerBase
    {
        [HttpGet("exams")]
        public async Task<ExamCollection> GetAll(
            bool isActive,
            int categoryGeneralLookUpId,
            int disciplineGeneralLookUpId)
        {
            return await Mediator.Send(new GetAllExamsQuery()
            {
                ISActive = isActive,
                CategoryGeneralLookUpId = categoryGeneralLookUpId,
                DisciplineGeneralLookUpId = disciplineGeneralLookUpId
            });
        }

        [HttpGet("exams/{examId}")]
        public async Task<ExamDto> GetById(string examId)
        {
            return await Mediator.Send(new GetExamWithIdQuery() { ExamId = examId });
        }

        [HttpGet("exams/activeCount")]
        public async Task<ActiveExamCount> GetActiveExamCount()
        {
            return await Mediator.Send(new GetActiveExamsCountQuery());
        }

        [HttpPost("exams/create")]
        public async Task<string> Create(ExamDto exam)
        {
            return await Mediator.Send(new CreateExamCommand() { Exam = exam });
        }

        [HttpPut("exams/update/{examId}")]
        public async Task<string> Update(string examId, ExamDto request)
        {
            return examId != request.UniqueId
                ? Constant.CommandsReturnStatus.NotFound
                : await Mediator.Send(new UpdateExamCommand() { Exam = request });
        }

        [HttpGet("exams/export/{examId}")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        public async Task<IActionResult> Export(string examId)
        {
            MemoryStream memoryStream = await Mediator.Send(new ExportExamCommand { ExamId = examId });

            return File(memoryStream, "application/octet-stream", "exam.xlsx");
        }

        [HttpDelete("exams/delete/{examId}")]
        public async Task<string> Delete(string examId)
        {
            return await Mediator.Send(new DeleteExamCommand { ExamId = examId });
        }

        [HttpGet("exams/getAllFonts")]
        public async Task<FontCollection> GetAllFonts()
        {
            return await Mediator.Send(new GetAllExamFontsQuery());
        }
    }
}
