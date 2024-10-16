using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Commands.Create;

public record CreateMultipleQuestionsCommand : IRequest<bool>
{
    public required string ExamId { get; set; }

    public QuestionCollection QuestionCollection { get; set; } = new();
}

public class CreateMultipleQuestionsCommandHandler : IRequestHandler<CreateMultipleQuestionsCommand, bool>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public CreateMultipleQuestionsCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(CreateMultipleQuestionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exam = await _context.Exams.FirstOrDefaultAsync(exam => exam.UniqueId == request.ExamId, cancellationToken) ?? throw new NotFoundException(nameof(Exam), request.ExamId);

            var question = _mapper.Map<List<Question>>(request.QuestionCollection.Questions);

            question.ForEach(q => q.Question_ExamID = exam.Id);

            await _context.Questions.AddRangeAsync(question, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(false);
            throw;
        }
    }

    #endregion
}
