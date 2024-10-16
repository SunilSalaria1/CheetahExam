using CheetahExam.WebUI.Shared.Common.Models.Companies;

namespace CheetahExam.Application.Companies.Queries.GetCompanyWithUserId;

public record GetCompanyWithUserIdQuery : IRequest<CompanyDto>
{
    public required string UserId { get; set; }
}

public class GetCompanyWithUserIdQueryHandler : IRequestHandler<GetCompanyWithUserIdQuery, CompanyDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetCompanyWithUserIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<CompanyDto> Handle(GetCompanyWithUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.UniqueId == request.UserId, cancellationToken);

        if (user is null) { return new(); /*use custom exception*/}

        var company = await _context.Companies
            .AsNoTracking()
            .Include(company => company.Media)
            .FirstOrDefaultAsync(company => company.Id == user.User_CompanyID, cancellationToken);

        if (company is null) { return new(); /*use custom exception*/}

        return _mapper.Map<CompanyDto>(company);
    }

    #endregion
}
