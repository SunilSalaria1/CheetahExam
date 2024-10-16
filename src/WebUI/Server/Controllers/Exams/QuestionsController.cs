using CheetahExam.Application.Questions.Commands.Create;
using CheetahExam.Application.Questions.Commands.Delete;
using CheetahExam.Application.Questions.Commands.Update;
using CheetahExam.Application.Questions.Commands.UpdateDisplayOrder;
using CheetahExam.Application.Questions.Queries.GetAll;
using CheetahExam.Application.Questions.Queries.GetAllChildQuestionsWithParentId;
using CheetahExam.Application.Questions.Queries.GetAllQuestionDisplayOrderWithExamId;
using CheetahExam.Application.Questions.Queries.GetLargestDisplayOrderWithExamId;
using CheetahExam.Application.Questions.Queries.GetWithd;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CheetahExam.WebUI.Server.Controllers.Exams;

[Route("/")]
[Authorize(Roles = $"{Constant.UserRoles.SuperAdmin},{Constant.UserRoles.Admin}")]
public class QuestionsController : ApiControllerBase
{
    [HttpPost("questions")]
    [OpenApiOperation("Create a new question", "Create a new Question under an exam.")]
    public async Task<string> Create(QuestionDto question)
    {
        return await Mediator.Send(new CreateQuestionCommand() { Question = question });
    }
    
    [HttpPost("questions/multiple/{examId}")]
    [OpenApiOperation("Create a new question collection", "Add Question Collections under an exam.")]
    public async Task<bool> CreateMultiple(string examId, QuestionCollection questionCollection)
    {
        return await Mediator.Send(new CreateMultipleQuestionsCommand() { ExamId = examId, QuestionCollection = questionCollection });
    }

    [HttpGet("questions/{questionId}")]
    [OpenApiOperation("Get a question", "Get a Question with id.")]
    public async Task<QuestionDto> GetById(string questionId)
    {
        return await Mediator.Send(new GetQuestionWithIdQuery() { QuestionId = questionId });
    }
    
    [HttpGet("questions/parent/{questionId}")]
    [OpenApiOperation("Get a List of question", "Get a List of Question with ParentQuestionId.")]
    public async Task<List<QuestionDto>> GetChildQuestionsWithId(string questionId)
    {
        return await Mediator.Send(new GetAllChildQuestionsWithParentIdQuery() { QuestionId = questionId });
    }

    [HttpGet("questions")]
    [OpenApiOperation("Get all questions", "Get a list of Question with exam Id.")]
    public async Task<QuestionCollection> GetAllByExamId(string examId)
    {
        return await Mediator.Send(new GetAllQuestionWithExamIdQuery() { ExamId = examId });
    }

    [HttpGet("questions/displayOrders/{examId}")]
    [OpenApiOperation("Get Display orders", "Get a list of Display orders with exam Id.")]
    public async Task<List<DisplayOrderDto>> GetDisplayOrders(string examId)
    {
        return await Mediator.Send(new GetAllQuestionDisplayOrderWithExamId() { ExamId = examId });
    }

    [HttpGet("questions/largestDisplayOrder/{examId}")]
    [OpenApiOperation("Get Largest Display order", "Get the largest Display order from question list with exam Id.")]
    public async Task<int> GetLargestDisplayOrder(string examId)
    {
        return await Mediator.Send(new GetLargestDisplayOrderWithExamIdQuery() { ExamId = examId });
    }

    [HttpPut("questions/{questionId}")]
    [OpenApiOperation("Update a Question", "Update the Question with exam Id.")]
    public async Task<string> Update(string questionId, QuestionDto question)
    {
        return await Mediator.Send(new UpdateQuestionCommand()
        {
            Question = question,
            QuestionId = questionId
        });
    }

    [HttpPut("questions/update/displayOrders/{examId}")]
    [OpenApiOperation("Update Questions Display orders", "Update the Question Display orders with exam Id.")]
    public async Task<bool> UpdateDisplayOrders(string examId, [FromBody] List<DisplayOrderDto> displayOrders)
    {
        return await Mediator.Send(new UpdateQuestionDisplayOrderCommand() { ExamId = examId, QuestionDisplayOrders = displayOrders });
    }

    [HttpDelete("questions/delete/{questionId}")]
    [OpenApiOperation("Delete a Question", "Delete an exisiting Question with Id.")]
    public async Task<string> Delete(string questionId)
    {
        return await Mediator.Send(new DeleteQuestionCommand() { QuestionId = questionId });
    }
}
