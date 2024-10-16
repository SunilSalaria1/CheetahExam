using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Commands.Create;

public record CreateQuestionCommand : IRequest<string>
{
    public QuestionDto? Question { get; set; }
}

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, string>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public CreateQuestionCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)

    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<string> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var exam = await _context.Exams.Where(exam => exam.UniqueId.Equals(request.Question.ExamUniqueId)).FirstOrDefaultAsync(cancellationToken);

        if (exam is null)
            throw new NotFoundException(nameof(Exam), request.Question.ExamUniqueId);

        var question = _mapper.Map<Question>(request.Question);

        question.Question_ExamID = exam.Id;

        await _context.Questions.AddAsync(question, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return question.UniqueId;
    }

    #endregion

}
