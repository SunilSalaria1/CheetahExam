using static CheetahExam.WebUI.Shared.Constants.Constant;

namespace CheetahExam.Application.Exams.Commands.Delete;

public record DeleteExamCommand : IRequest<string>
{
    public required string ExamId { get; set; }
}

public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, string>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public DeleteExamCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Method

    public async Task<string> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
    {
        Exam? exam = await _context.Exams
            .FirstOrDefaultAsync(x => x.UniqueId == request.ExamId, cancellationToken);

        if (exam != null)
        {
            exam.ISArchive = !exam.ISArchive;

            exam.ISActive = false;

            await _context.SaveChangesAsync(cancellationToken);

            return exam.ISArchive ? CommandsReturnStatus.Deleted : CommandsReturnStatus.Retrieved;
        }

        return CommandsReturnStatus.NotFound;
    }

    #endregion
}
