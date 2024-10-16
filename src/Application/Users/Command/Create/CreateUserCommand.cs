using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Users;
using Microsoft.Extensions.Configuration;

namespace CheetahExam.Application.Users.Command.Create;

public record CreateUserCommand() : IRequest<Result>
{
    public required string CompanyId { get; init; }

    public required UserDto User { get; init; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    #endregion

    #region Ctor

    public CreateUserCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    #endregion

    #region Method

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        List<string> errors = new();

        string password = _configuration["UserPassword"]!;

        try
        {
            var company = await _context.Companies.FirstOrDefaultAsync(company => company.UniqueId == request.CompanyId, cancellationToken) ?? throw new NotFoundException("Company", request.CompanyId);

            if (string.IsNullOrWhiteSpace(request.User?.Email)) throw new BadRequestException("Email is not available");

            var existingUser = await _context.Users.FirstOrDefaultAsync(
                    user => user.Email.Trim().ToLower() == request.User.Email.Trim().ToLower() &&
                    user.User_CompanyID == company.Id, cancellationToken);

            if (existingUser != null) throw new AlreadyExistsException("User", request.User.Email.ToLower());

            var user = _mapper.Map<User>(request.User);
            CreateNewUser(password, company, user);

            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            // Create roles
            var roles = await _context.Roles
                .Where(x => request.User.Roles.Contains(x.Name))
                .ToListAsync(cancellationToken);

            if (!roles.Any()) throw new BadRequestException("Unknown role");

            _context.UserRoleMappers.AddRange(roles.Select(role => new UserRoleMapper
            {
                UserRoleMapper_UserID = user.Id,
                UserRoleMapper_RoleID = role.Id
            }));

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (NotFoundException nf) { errors.Add(nf.Message); }
        catch (BadRequestException br) { errors.Add(br.Message); }
        catch (AlreadyExistsException ae) { errors.Add(ae.Message); }
        catch (Exception ex) { errors.Add(ex.Message); }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    private static void CreateNewUser(string password, Company? company, User user)
    {
        user.UserName = user.Email.ToLowerInvariant();
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        user.User_CompanyID = company?.Id;
    }

    #endregion
}
