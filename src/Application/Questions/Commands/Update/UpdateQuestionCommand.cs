using CheetahExam.WebUI.Shared.Common.Models.Exams;
using static CheetahExam.WebUI.Shared.Constants.Constant;

namespace CheetahExam.Application.Questions.Commands.Update;

public record UpdateQuestionCommand : IRequest<string>
{
    public required string QuestionId { get; set; }

    public QuestionDto? Question { get; set; }
}

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, string>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public UpdateQuestionCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<string> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions.Where(question => question.UniqueId == request.QuestionId)
                                .Include(question => question.Media)
                                .Include(question => question.Options)
                                .ThenInclude(option => option.Media)
                                .FirstOrDefaultAsync(cancellationToken);

        if (question is null) { return CommandsReturnStatus.NotFound; }

        question = _mapper.Map(request.Question, question);

        _context.Questions.Update(question);

        await _context.SaveChangesAsync(cancellationToken);

        return "Updated";
    }

    #endregion
}
