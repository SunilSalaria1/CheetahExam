
namespace CheetahExam.Application.Roles.Command.Create;

public record CreateRoleCommand : IRequest<Role>
{ 
    public int UserId { get; set; }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Role>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public CreateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Method

    public Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        
    }

    #endregion
    
}
