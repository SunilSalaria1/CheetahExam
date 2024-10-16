namespace CheetahExam.Application.Questions.Queries.GetLargestDisplayOrderWithExamId;

public record GetLargestDisplayOrderWithExamIdQuery : IRequest<int>
{
    public required string ExamId { get; set; }
}

public class GetLargestDisplayOrderWithExamIdQueryHandler : IRequestHandler<GetLargestDisplayOrderWithExamIdQuery, int>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public GetLargestDisplayOrderWithExamIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<int> Handle(GetLargestDisplayOrderWithExamIdQuery request, CancellationToken cancellationToken)
    {
        int displayOrder = 0;

        var exam = await _context.Exams.Where(exam => exam.UniqueId == request.ExamId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (exam == null) { return 0; }

        displayOrder = await _context.Questions.Where(question => question.Question_ExamID == exam.Id)
            .OrderByDescending(question => question.DisplayOrder)
            .AsNoTracking().Select(question => question.DisplayOrder)
            .FirstOrDefaultAsync(cancellationToken);

        return displayOrder;
    }

    #endregion
}
