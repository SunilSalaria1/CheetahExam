using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Queries.GetAllChildQuestionsWithParentId;

public class GetAllChildQuestionsWithParentIdQuery : IRequest<List<QuestionDto>>
{
    public required string QuestionId { get; set; }
}
public class GetAllChildQuestionsWithParentIdQueryHandler : IRequestHandler<GetAllChildQuestionsWithParentIdQuery, List<QuestionDto>>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetAllChildQuestionsWithParentIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region methods

    public async Task<List<QuestionDto>> Handle(GetAllChildQuestionsWithParentIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _context.Questions.Where(question => question.ParentQuestionId == request.QuestionId)
                        .Include(question => question.Media)
                        .Include(question => question.Options)
                        .ThenInclude(question => question.Media)
                        .ToListAsync(cancellationToken);

        return _mapper.Map<List<QuestionDto>>(questions);
    }

    #endregion
}
