using CheetahExam.WebUI.Shared.Common.Models.Users;

namespace CheetahExam.Application.Roles.Query.GetRoleWithUserId;

public record GetRoleWithUserIdQuery : IRequest<RoleDto>
{
    public required string UserId { get; init; }
}

public class GetRoleWithUserIdQueryHandler : IRequestHandler<GetRoleWithUserIdQuery, RoleDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetRoleWithUserIdQueryHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Handle

    public async Task<RoleDto> Handle(GetRoleWithUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FirstOrDefault(x => x.UniqueId == request.UserId);

        if (user == null) return new();

        var userRoleMapper = _context.UserRoleMappers.FirstOrDefault(x => x.UserRoleMapper_UserID == user.Id);

        if (userRoleMapper == null) return new();

        var role = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userRoleMapper.UserRoleMapper_RoleID, cancellationToken);

        return role == null ? new() : _mapper.Map<RoleDto>(role);
    }

    #endregion
}
