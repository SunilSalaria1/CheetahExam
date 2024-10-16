using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Exams.Queries.GetActiveExamsCount;

public record GetActiveExamsCountQuery : IRequest<ActiveExamCount> { };

public class GetActiveExamsCountQueryHandler : IRequestHandler<GetActiveExamsCountQuery, ActiveExamCount> 
{
    private readonly IApplicationDbContext _context;

    public GetActiveExamsCountQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ActiveExamCount> Handle(GetActiveExamsCountQuery request, CancellationToken cancellationToken)
    {
        var exams = await _context.Exams.Where(exam => !exam.ISArchive).ToListAsync();

        var activeExams = new ActiveExamCount
        {
            ActiveExams = exams.Count(exam => exam.ISActive),
            TotalExams = exams.Count()
        };

        return activeExams;
    }
}
