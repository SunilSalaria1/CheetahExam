using CheetahExam.Application.Common.Services.Account;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Accounts;

namespace CheetahExam.Application.Accounts.Commands.AuthenticateAccount;

public record AuthenticateAccountCommand : IRequest<ResultDto>
{
    public required LoginDto Login { get; set; }
}

public class AuthenticateAccountCommandHandler : IRequestHandler<AuthenticateAccountCommand, ResultDto>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IAccountService _accountService;

    #endregion

    #region Ctor

    public AuthenticateAccountCommandHandler(
        IApplicationDbContext context,
        IAccountService accountService)
    {
        _context = context;
        _accountService = accountService;
    }

    #endregion

    #region Method

    public async Task<ResultDto> Handle(AuthenticateAccountCommand request, CancellationToken cancellationToken)
    {
        List<string> roles = new();

        if (string.IsNullOrWhiteSpace(request.Login.Email) || string.IsNullOrWhiteSpace(request.Login.Password))
        {
            return new()
            {
                Succeeded = false,
                Errors = new List<string> { "Invalid request" }
            };
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName!.ToLower() == request.Login.Email.ToLower(), cancellationToken);

        if (user == null)
        {
            return new()
            {
                Succeeded = false,
                Errors = new List<string> { "Invalid username or password" }
            };
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Login.Password, user.HashedPassword))
        {
            return new()
            {
                Succeeded = false,
                Errors = new List<string> { "Invalid username or password" }
            };
        }

        var userRoleMapper = await _context.UserRoleMappers.FirstOrDefaultAsync(ur => ur.UserRoleMapper_UserID == user.Id, cancellationToken);

        if (userRoleMapper != null)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == userRoleMapper.UserRoleMapper_RoleID, cancellationToken);

            roles.Add(role!.Name);
        }

        return new()
        {
            Token = _accountService.GenerateToken(user.UniqueId, $"{user.FirstName} {user.LastName}", roles),
            Succeeded = true,
            UserId = user.UniqueId,
        };
    }

    #endregion
}
