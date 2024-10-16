using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Exams.Queries.GetAllExams;

public record GetAllExamsQuery : IRequest<ExamCollection>
{
    public bool ISActive { get; init; }

    public int? CategoryGeneralLookUpId { get; init; }

    public int? DisciplineGeneralLookUpId { get; init; }
}

public class GetAllExamsQueryHandler : IRequestHandler<GetAllExamsQuery, ExamCollection>
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public GetAllExamsQueryHandler(
        IMapper mapper,
        IApplicationDbContext context
        )
    {
        _mapper = mapper;
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<ExamCollection> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
    {
        int categoryGeneralLookUpId = request.CategoryGeneralLookUpId ?? 0;
        int disciplineGeneralLookUpId = request.DisciplineGeneralLookUpId ?? 0;

        List<Exam> exams = await _context.Exams
                .AsNoTracking()
                .Include(x => x.Media)
                .Include(x => x.Category_GeneralLookUp)
                .Include(x => x.Discipline_GeneralLookUp)
                .Include(x => x.Result_GeneralLookUp)
                .Include(x => x.Questions)
                .Where(exam =>
                    (exam.ISArchive == false) &&
                    (!request.ISActive || exam.ISActive)
                    && (categoryGeneralLookUpId == 0 || exam.Category_GeneralLookUpID == categoryGeneralLookUpId)
                    && (disciplineGeneralLookUpId == 0 || exam.Discipline_GeneralLookUpID == disciplineGeneralLookUpId))
                .OrderByDescending(x => x.ExamDate).ToListAsync(cancellationToken);

       
        var examCollection = new ExamCollection
        {
            Exams = _mapper.Map<List<ExamDto>>(exams)
        };

        return examCollection;
    }

    #endregion
}
