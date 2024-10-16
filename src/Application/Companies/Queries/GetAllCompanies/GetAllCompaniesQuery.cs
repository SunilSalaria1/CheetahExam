using CheetahExam.WebUI.Shared.Common.Models.Companies;

namespace CheetahExam.Application.Companies.Queries.GetAllCompanies;

public record GetAllCompaniesQuery : IRequest<CompanyCollectionDto>
{
    public bool ISActive { get; set; }
};

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, CompanyCollectionDto>
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public GetAllCompaniesQueryHandler(
        IMapper mapper,
        IApplicationDbContext context
        )
    {
        _mapper = mapper;
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<CompanyCollectionDto> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        CompanyCollectionDto companyCollection = new();

        List<Company> companies = await _context.Companies
            .AsNoTracking()
            .Include(company => company.Media)
            .Where(company => company.ISActive == request.ISActive)
            .ToListAsync(cancellationToken);

        companyCollection.Companies = _mapper.Map<List<CompanyDto>>(companies);

        return companyCollection;
    }

    #endregion
}
