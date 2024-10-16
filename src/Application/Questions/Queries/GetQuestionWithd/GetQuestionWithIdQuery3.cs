using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Queries.GetWithd;

public class GetQuestionWithIdQuery : IRequest<QuestionDto>
{
    public string QuestionId { get; set; }
}

public class GetQuestionWithIdQueryHandler : IRequestHandler<GetQuestionWithIdQuery, QuestionDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetQuestionWithIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<QuestionDto> Handle(GetQuestionWithIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.Where(question => question.UniqueId == request.QuestionId).AsNoTracking()
                                .Include(question => question.Media)
                                .Include(question => question.Options)
                                .ThenInclude(question => question.Media).FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<QuestionDto>(question);
    }

    #endregion
}
