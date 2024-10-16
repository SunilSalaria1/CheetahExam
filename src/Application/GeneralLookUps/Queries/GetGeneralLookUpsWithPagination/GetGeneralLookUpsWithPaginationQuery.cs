using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;
using static CheetahExam.WebUI.Shared.Constants.Constant;

namespace CheetahExam.Application.GeneralLookups.Queries.GetGeneralLookUpsWithPagination;

public record GetGeneralLookUpsWithPaginationQuery : IRequest<List<GeneralLookUpDto>>
{
    public bool ISIncludeArchived { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}

public class GetGeneralLookUpsWithPaginationHandler : IRequestHandler<GetGeneralLookUpsWithPaginationQuery, List<GeneralLookUpDto>>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetGeneralLookUpsWithPaginationHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<List<GeneralLookUpDto>> Handle(GetGeneralLookUpsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;

        var generalLookups = await _context.GeneralLookUps
            .AsNoTracking()
            .Where(generalLookup => generalLookup.Type != LookUpTypes.Media_Type && generalLookup.Type != LookUpTypes.Result_Type
                         && (request.ISIncludeArchived || !generalLookup.ISArchive))
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<GeneralLookUpDto>>(generalLookups);
    }

    #endregion
}
