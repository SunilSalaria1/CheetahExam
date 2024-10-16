using CheetahExam.WebUI.Shared.Common.Models.Companies;

namespace CheetahExam.Application.Companies.Queries.GetCompanyWithId;

public record GetCompanyWithIdQuery : IRequest<CompanyDto>
{
    public required string CompanyId { get; init; }
}

public class GetCompanyWithIdQueryHandler : IRequestHandler<GetCompanyWithIdQuery, CompanyDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetCompanyWithIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<CompanyDto> Handle(GetCompanyWithIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .Include(company => company.Media)
            .FirstOrDefaultAsync(company => company.UniqueId == request.CompanyId, cancellationToken);

        return company is null ? new CompanyDto() : _mapper.Map<CompanyDto>(company);
    }

    #endregion
}
