using CheetahExam.WebUI.Shared.Common.Models.Users;

namespace CheetahExam.Application.Users.Queries;

public record GetUserQuery : IRequest<UserDto>
{
    public required string UserId { get; init; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetUserQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Method

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
           .Include(u => u.UserRoleMappers)
            .ThenInclude(x=> x.UserRoleMapper_Role)
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.UniqueId == request.UserId, cancellationToken);

        if (user is null) return new();

        var userDto = _mapper.Map<UserDto>(user);

        var userRoles = user.UserRoleMappers
            .Select(userRoleMapper => userRoleMapper.UserRoleMapper_Role?.Name)
            .ToList();

        if (userRoles == null) return new();

        userDto.Roles = userRoles!;

        return userDto;
    }

    #endregion
}
