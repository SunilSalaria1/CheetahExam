using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Users;

namespace CheetahExam.Application.Users.Command.Update;

public record UpdateUserCommand : IRequest<Result>
{
    public required string CompanyId { get; set; }

    public required UserDto User { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public UpdateUserCommandHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Method

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();

        try
        {
            if (string.IsNullOrWhiteSpace(request.User.UniqueId)) throw new BadRequestException("Unique Id is not available");

            var user = await _context.Users
                .Include(user => user.UserRoleMappers)
                .FirstOrDefaultAsync(x => x.UniqueId == request.User.UniqueId, cancellationToken) ?? throw new NotFoundException("User", request.User.UniqueId);

            var company = await _context.Companies
                .FirstOrDefaultAsync(x => x.UniqueId == request.CompanyId, cancellationToken) ?? throw new NotFoundException("Company", request.CompanyId);

            if (string.IsNullOrWhiteSpace(request.User.Email)) throw new BadRequestException("Email is not available");

            var existingUser = await _context.Users
               .FirstOrDefaultAsync(x => x.UniqueId != request.User.UniqueId &&
                                    x.User_CompanyID == company.Id && x.Email.Trim().ToLower() == request.User.Email.Trim().ToLower(), cancellationToken);

            if (existingUser != null) throw new AlreadyExistsException("User", request.User.Email.ToLower());

            user = _mapper.Map(request.User, user);

            user.UserName = request.User.Email.ToLower();

            // Update roles
            var roles = await _context.Roles
                .Where(role => request.User.Roles.Contains(role.Name))
                .ToListAsync(cancellationToken);

            if (!roles.Any()) throw new BadRequestException("Unknown role");

            var existingRoleIds = new HashSet<int?>(user.UserRoleMappers.Select(ur => ur.UserRoleMapper_RoleID));

            var rolesToAdd = roles.Where(role => !existingRoleIds.Contains(role.Id))
                          .Select(role => new UserRoleMapper { UserRoleMapper_UserID = user.Id, UserRoleMapper_RoleID = role.Id })
                          .ToList();

            var rolesToRemove = user.UserRoleMappers.Where(ur => !roles.Select(r => (int?)r.Id).Contains(ur.UserRoleMapper_RoleID)).ToList();

            _context.UserRoleMappers.AddRange(rolesToAdd);
            _context.UserRoleMappers.RemoveRange(rolesToRemove);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (NotFoundException nf) { errors.Add(nf.Message); }
        catch (BadRequestException br) { errors.Add(br.Message); }
        catch (AlreadyExistsException ae) { errors.Add(ae.Message); }
        catch (Exception ex) { errors.Add(ex.Message); }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    #endregion
}
