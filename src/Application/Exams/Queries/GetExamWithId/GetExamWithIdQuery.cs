using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Exams.Queries.GetExamWithId;

public class GetExamWithIdQuery : IRequest<ExamDto>
{
    public required string ExamId { get; set; }
}

public class GetExamWithIdQueryHandler : IRequestHandler<GetExamWithIdQuery, ExamDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetExamWithIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<ExamDto> Handle(GetExamWithIdQuery request, CancellationToken cancellationToken)
    {
        var exam = await _context.Exams
            .Include(exam => exam.Media)
            .Include(exam => exam.ExamResultOptions)
            .Include(exam => exam.Questions)
            .FirstOrDefaultAsync(exam => exam.UniqueId == request.ExamId, cancellationToken);

        var examDto = _mapper.Map<ExamDto>(exam);

        return examDto;

    }

    #endregion
}
