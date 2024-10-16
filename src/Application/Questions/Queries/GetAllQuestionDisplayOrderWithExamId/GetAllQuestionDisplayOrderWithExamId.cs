using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Queries.GetAllQuestionDisplayOrderWithExamId;

public record GetAllQuestionDisplayOrderWithExamId : IRequest<List<DisplayOrderDto>>
{
    public required string ExamId { get; set; }
}

public record GetAllQuestionDisplayOrderWithExamIdHandler : IRequestHandler<GetAllQuestionDisplayOrderWithExamId, List<DisplayOrderDto>>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public GetAllQuestionDisplayOrderWithExamIdHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<List<DisplayOrderDto>> Handle(GetAllQuestionDisplayOrderWithExamId request, CancellationToken cancellationToken)
    {
        List<DisplayOrderDto> questionDisplayOrders = new();

        var exam = await _context.Exams.Where(exam => exam.UniqueId == request.ExamId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (exam is null) { return questionDisplayOrders; } // through exception

        questionDisplayOrders = await _context.Questions.Where(question => question.Question_ExamID == exam.Id)
                .Select(question => new DisplayOrderDto()
                {
                    UniqueId = question.UniqueId,
                    DisplayOrder = question.DisplayOrder
                }).ToListAsync(cancellationToken);

        return questionDisplayOrders;
    }

    #endregion
}
