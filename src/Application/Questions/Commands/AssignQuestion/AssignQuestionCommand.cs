using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Commands.AssignQuestion;

public record AssignQuestionCommand : IRequest<bool>
{
    public List<QuestionDto> Questions { get; set; }
}
public class AssignQuestionCommandHandler : IRequestHandler<AssignQuestionCommand, bool>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public AssignQuestionCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region methods

    public async Task<bool> Handle(AssignQuestionCommand request, CancellationToken cancellationToken)
    {
        List<Question> questions = new();

        foreach (var question in request.Questions)
        {
            var questionToBeUpdated = await _context.Questions.Where(question => question.UniqueId == question.UniqueId)
                                .Include(question => question.Media)
                                .Include(question => question.Options)
                                .ThenInclude(option => option.Media)
                                .FirstOrDefaultAsync(cancellationToken);

            if (questionToBeUpdated is null) { return false; }

            questionToBeUpdated = _mapper.Map(question, questionToBeUpdated);

            _context.Questions.Update(questionToBeUpdated);

            await _context.SaveChangesAsync(cancellationToken);
        }

        _context.Questions.UpdateRange(questions);

        return await Task.FromResult(true);
    }

    #endregion
}
