using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Exams.Queries.GetAllExamFonts;

public record GetAllExamFontsQuery : IRequest<FontCollection>;

public class GetAllExamFontsQueryHandler : IRequestHandler<GetAllExamFontsQuery, FontCollection>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetAllExamFontsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Method

    public async Task<FontCollection> Handle(GetAllExamFontsQuery request, CancellationToken cancellationToken)
    {
        var fonts = await _context.Fonts
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new FontCollection()
        {
            Fonts = _mapper.Map<List<FontDto>>(fonts)
        };
    }

    #endregion
}
