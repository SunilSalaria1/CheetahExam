using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;

namespace CheetahExam.Application.Users.Command.Update;

public record UpdateUserRoleCommand : IRequest<Result>
{
    public required string UserId { get; set; }

    public required List<string> RoleNames { get; set; }
}

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public UpdateUserRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Method

    public async Task<Result> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();

        try
        {
            var user = await _context.Users
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.UniqueId == request.UserId, cancellationToken) ?? throw new NotFoundException("User", request.UserId);

            var roles = await _context.Roles
                                .AsNoTracking()
                                .Where(x => request.RoleNames.Contains(x.Name))
                                .ToListAsync(cancellationToken);

            if (!roles.Any()) throw new BadRequestException("Unknown role");

            var rolesToAdd = roles.Where(role => !user.UserRoleMappers.Any(ur => ur.UserRoleMapper_RoleID == role.Id))
                                  .Select(role => new UserRoleMapper { UserRoleMapper_UserID = user.Id, UserRoleMapper_RoleID = role.Id })
                                  .ToList();

            var rolesToRemove = user.UserRoleMappers
                                    .Where(ur => !roles.Any(role => role.Id == ur.UserRoleMapper_RoleID))
                                    .ToList();

            _context.UserRoleMappers.AddRange(rolesToAdd);
            _context.UserRoleMappers.RemoveRange(rolesToRemove);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (NotFoundException nf) { errors.Add(nf.Message); }
        catch (BadRequestException br) { errors.Add(br.Message); }
        catch (Exception ex) { errors.Add(ex.Message); }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    #endregion
}
