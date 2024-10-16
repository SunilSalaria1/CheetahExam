using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;

namespace CheetahExam.Application.GeneralLookups.Commands.Retrieve;

public record RetrieveGeneralLoopUpCommand : IRequest<Result>
{
    public required string UniqueId { get; set; }
}

public class RetrieveGeneralLoopUpCommandHandler : IRequestHandler<RetrieveGeneralLoopUpCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public RetrieveGeneralLoopUpCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<Result> Handle(RetrieveGeneralLoopUpCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();

        try
        {
            GeneralLookUp generalLookUp = await _context.GeneralLookUps
                .FirstOrDefaultAsync(generalLookUp => generalLookUp.UniqueId == request.UniqueId, cancellationToken)
                ?? throw new NotFoundException("GeneralLookUp", request.UniqueId); ;

            generalLookUp.ISArchive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (NotFoundException nf)
        {
            errors.Add(nf.Message);
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
        }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    #endregion
}
