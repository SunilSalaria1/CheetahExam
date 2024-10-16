using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MediatR;

namespace CheetahExam.Application.Options.Commands.UpdateOptionsDisplayOrder;

public record UpdateOptionsDisplayOrderCommand : IRequest<bool>
{
    public required string QuestionId { get; set; }

    public List<DisplayOrderDto> OptionDisplayOrders { get; set; } = null!;
}
public class UpdateOptionsDisplayOrderCommandHandler : IRequestHandler<UpdateOptionsDisplayOrderCommand, bool>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public UpdateOptionsDisplayOrderCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<bool> Handle(UpdateOptionsDisplayOrderCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
               .Where(question => question.UniqueId == request.QuestionId)
               .Include(question => question.Options)
               .FirstOrDefaultAsync(cancellationToken);

        if (question is null || question.Options is null) { return false; }

        foreach (var newDisplayOrder in request.OptionDisplayOrders)
        {
            var existingOption = question.Options.FirstOrDefault(exisitng => exisitng.UniqueId == newDisplayOrder.UniqueId);

            if (existingOption != null) { existingOption.DisplayOrder = newDisplayOrder.DisplayOrder; }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion
}
