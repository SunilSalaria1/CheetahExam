using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Questions.Commands.UpdateDisplayOrder;

public record UpdateQuestionDisplayOrderCommand : IRequest<bool>
{
    public required string ExamId { get; set; }

    public List<DisplayOrderDto> QuestionDisplayOrders { get; set; } = null!;
}

public class UpdateQuestionDisplayOrderCommandHandler : IRequestHandler<UpdateQuestionDisplayOrderCommand, bool>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public UpdateQuestionDisplayOrderCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(UpdateQuestionDisplayOrderCommand request, CancellationToken cancellationToken)
    {
        var exam = await _context.Exams
                .Where(exam => exam.UniqueId == request.ExamId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

        if (exam is null) { return false; }

        var exisitngQuestionOrders = await _context.Questions
            .Where(question => question.Question_ExamID == exam.Id)
            .ToListAsync(cancellationToken);

        foreach (var newDisplayOrder in request.QuestionDisplayOrders)
        {
            var existingQuestion = exisitngQuestionOrders.FirstOrDefault(exisitng => exisitng.UniqueId == newDisplayOrder.UniqueId);

            if (existingQuestion != null) {  existingQuestion.DisplayOrder = newDisplayOrder.DisplayOrder; }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion
}
