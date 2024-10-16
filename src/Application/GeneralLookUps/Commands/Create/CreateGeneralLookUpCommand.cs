using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;

namespace CheetahExam.Application.GeneralLookups.Commands.Create;

public record CreateGeneralLookUpCommand : IRequest<Result>
{
    public required GeneralLookUpDto GeneralLookUp { get; set; }
}

public class CreateGeneralLookUpCommandHandler : IRequestHandler<CreateGeneralLookUpCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public CreateGeneralLookUpCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<Result> Handle(CreateGeneralLookUpCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();

        GeneralLookUp generalLookUp = _mapper.Map<GeneralLookUp>(request.GeneralLookUp);

        try
        {
            if (string.IsNullOrWhiteSpace(generalLookUp.Value) || string.IsNullOrWhiteSpace(generalLookUp.Type)) throw new BadRequestException("Value or Type is required");

            GeneralLookUp? existingGeneralLookUp = await _context.GeneralLookUps.Where(lookUp => !string.IsNullOrWhiteSpace(lookUp.Value) &&
                                                            lookUp.Value.Trim().ToLower() == generalLookUp.Value.Trim().ToLower() &&
                                                            lookUp.Type.Trim().ToLower() == generalLookUp.Type.Trim().ToLower())
                                                            .FirstOrDefaultAsync(cancellationToken);

            if (existingGeneralLookUp is not null) throw new AlreadyExistsException($"Type & Value", $"{request.GeneralLookUp.Type} & {request.GeneralLookUp.Value}");

            await _context.GeneralLookUps.AddAsync(generalLookUp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (AlreadyExistsException ae)
        {
            errors.Add(ae.Message);
        }
        catch (BadRequestException br)
        {
            errors.Add(br.Message);
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
        }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    #endregion
}
