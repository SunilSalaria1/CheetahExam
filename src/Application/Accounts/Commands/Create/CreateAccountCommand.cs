using CheetahExam.Application.Common.Services.Account;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Accounts;

namespace CheetahExam.Application.Accounts.Commands.Create;

public record CreateAccountCommand : IRequest<ResultVm>
{
    public required RegisterVm User { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ResultVm>
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IAccountService _accountService;

    #endregion

    #region Ctor

    public CreateAccountCommandHandler(
        IMapper mapper, 
        IApplicationDbContext context,
        IAccountService accountService)
    {
        _mapper = mapper;
        _context = context;
        _accountService = accountService;
    }

    #endregion

    #region Method

    public async Task<ResultVm> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {

        #region remove this

        if (string.IsNullOrWhiteSpace(request.User.FirstName) ||
            string.IsNullOrWhiteSpace(request.User.Email) ||
            string.IsNullOrWhiteSpace(request.User.Password) ||
            string.IsNullOrWhiteSpace(request.User.ConfirmPassword) ||
            request.User.Password != request.User.ConfirmPassword)
        {
            return new() { Succeeded = false, Errors = new List<string>() { "Invalid request" } };
        }

        #endregion

        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLowerInvariant() == request.User.Email.ToLowerInvariant(), cancellationToken);
        
        if (existingUser != null) {  return new() { Succeeded = false, Errors = new List<string> { "Email already exists!" } }; }

        var user = _mapper.Map<User>(request.User);

        user.UserName = request.User.Email.ToLowerInvariant();
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.User.Password);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        List<string> roles = new(); // set role of the user first

        return new()
        {
            Token = _accountService.GenerateToken(user.UniqueId, $"{user.FirstName} {user.LastName}", roles),
            Succeeded = true,
            UserId = user.UniqueId
        };
    }

    #endregion
}
