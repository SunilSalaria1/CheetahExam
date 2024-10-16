using CheetahExam.WebUI.Shared.Common.Models.Users;

namespace CheetahExam.Application.Users.Query.GetCompanyUsersWithId;

public record GetCompanyUsersWithIdQuery : IRequest<UsersDto>
{
    public required string CompanyId { get; init; }

    public required List<string> Roles { get; init; }
}

public class GetUsersWithCompanyIdQueryHandler : IRequestHandler<GetCompanyUsersWithIdQuery, UsersDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public GetUsersWithCompanyIdQueryHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Handle

    public async Task<UsersDto> Handle(GetCompanyUsersWithIdQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(user => user.UserRoleMappers)
                .ThenInclude(userRoleMapper => userRoleMapper.UserRoleMapper_Role) 
            .Include(user => user.User_Company) 
            .Where(user => user.User_Company != null 
                && user.User_Company.UniqueId == request.CompanyId)
            .ToListAsync(cancellationToken);

        var usersDto = new UsersDto();

        foreach (var user in users)
        {
            var roles = user.UserRoleMappers
                .Where(userRoleMapper => request.Roles.Contains(userRoleMapper.UserRoleMapper_Role!.Name))
                .Select(userRoleMapper => userRoleMapper.UserRoleMapper_Role!.Name)
                .ToList();

            if (!roles.Any()) continue;

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles;
            usersDto.Users.Add(userDto);
        }

        return usersDto;
    }

    #endregion
}
