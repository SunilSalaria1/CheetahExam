using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Queries.GetAll;

public record GetAllQuestionWithExamIdQuery : IRequest<QuestionCollection>
{
    public required string ExamId { get; set; }
}
public class GetAllQuestionWithExamIdQueryHandler : IRequestHandler<GetAllQuestionWithExamIdQuery, QuestionCollection>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetAllQuestionWithExamIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)

    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<QuestionCollection> Handle(GetAllQuestionWithExamIdQuery request, CancellationToken cancellationToken)
    {
        QuestionCollection questionCollection = new();

        var exam = _context.Exams.Where(exam => exam.UniqueId == request.ExamId)
                        .AsNoTracking()
                        .FirstOrDefault();

        if (exam is not null)
        {
            var questions = await _context.Questions.Where(questions => questions.Question_ExamID == exam.Id)
                            .Include(question => question.Media)
                            .Include(question => question.Options)
                            .ThenInclude(question => question.Media)
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);

            questionCollection.Questions = _mapper.Map<List<QuestionDto>>(questions);
        }

        return questionCollection;
    }
    #endregion
}
