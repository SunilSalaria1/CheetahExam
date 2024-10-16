using CheetahExam.Domain.Entities;
using static CheetahExam.WebUI.Shared.Constants.Constant;

namespace CheetahExam.Application.Questions.Commands.Delete;

public record DeleteQuestionCommand : IRequest<string>
{
    public required string QuestionId { get; set; }
}

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, string>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public DeleteQuestionCommandHandler(
        IApplicationDbContext context)

    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<string> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.Where(question => question.UniqueId == request.QuestionId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cancellationToken);

        if (question is not null)
        {
            var mediaList = await _context.Media.Where(media => media.Media_QuestionID == question.Id)
                    .ToListAsync(cancellationToken);

            mediaList.ForEach(media => _context.Media.Remove(media));

            var options = await _context.Options.Where(option => option.Option_QuestionID == question.Id)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

            foreach (var option in options)
            {
                await _context.Media.Where(media => media.Media_OptionID == option.Id)
                        .ForEachAsync(media => _context.Media.Remove(media), cancellationToken);
            }

            _context.Options.RemoveRange(options);

            _context.Questions.Remove(question);

            await _context.SaveChangesAsync(cancellationToken);
        }

        return CommandsReturnStatus.Deleted;
    }

    #endregion
}
