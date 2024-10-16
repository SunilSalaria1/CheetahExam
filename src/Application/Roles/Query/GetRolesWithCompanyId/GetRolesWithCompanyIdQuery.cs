using CheetahExam.WebUI.Shared.Common.Models.Users;

namespace CheetahExam.Application.Roles.Query.GetRolesWithCompanyId;

public record GetRolesWithCompanyIdQuery : IRequest<RolesDto>
{
    public required string CompanyId { get; init; }
}

public class GetRolesWithCompanyIdQueryHandler : IRequestHandler<GetRolesWithCompanyIdQuery, RolesDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetRolesWithCompanyIdQueryHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Handle

    public async Task<RolesDto> Handle(GetRolesWithCompanyIdQuery request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return roles.Any()
            ? new RolesDto { Roles = _mapper.Map<List<RoleDto>>(roles) }
            : new RolesDto();
    }

    #endregion
}
