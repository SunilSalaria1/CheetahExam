using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;

namespace CheetahExam.Application.Companies.Commands.Delete;

public record DeleteCompanyCommand : IRequest<Result>
{
    public required string UniqueId { get; set; }
}

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public DeleteCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Method

    public async Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();

        try
        {
            Company company = await _context.Companies
                                        .FirstOrDefaultAsync(company => company.UniqueId == request.UniqueId, cancellationToken)
                                        ?? throw new NotFoundException("Company", request.UniqueId);

            company.ISActive = !company.ISActive;

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (NotFoundException nf) { errors.Add(nf.Message); }
        catch (Exception ex) { errors.Add(ex.Message); }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    #endregion
}
