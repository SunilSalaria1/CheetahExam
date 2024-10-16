using CheetahExam.WebUI.Shared.Common.Models;

namespace CheetahExam.Application.GeneralLookups.Queries.GetGeneralLoopUpsWithType;

public record GetGeneralLoopUpsWithTypeQuery : IRequest<KeyValue[]>
{
    public required string GeneralLookUpType { get; init; }
}

public class GetGeneralLoopUpsWithTypeQueryHandler : IRequestHandler<GetGeneralLoopUpsWithTypeQuery, KeyValue[]>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public GetGeneralLoopUpsWithTypeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<KeyValue[]> Handle(GetGeneralLoopUpsWithTypeQuery request, CancellationToken cancellationToken)
    {
        return await _context.GeneralLookUps
             .AsNoTracking()
             .Where(generalLoopUp => generalLoopUp.Type.Trim() == request.GeneralLookUpType)
             .Select(generalLoopUp => new KeyValue()
             {
                 Key = generalLoopUp.Id,
                 Value = generalLoopUp.Value
             }).ToArrayAsync(cancellationToken);

    }

    #endregion
}
