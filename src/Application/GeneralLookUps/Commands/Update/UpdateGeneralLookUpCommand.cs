using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;

namespace CheetahExam.Application.GeneralLookups.Commands.Update;

public record UpdateGeneralLookUpCommand : IRequest<Result>
{
    public required GeneralLookUpDto GeneralLookUp { get; set; }
}

public class UpdateGeneralLookUpCommandHandler : IRequestHandler<UpdateGeneralLookUpCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public UpdateGeneralLookUpCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<Result> Handle(UpdateGeneralLookUpCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();

        try
        {
            if (request.GeneralLookUp.UniqueId is null) throw new BadRequestException("Unique Id is not available");

            GeneralLookUp generalLookUp = await _context.GeneralLookUps
                    .FirstOrDefaultAsync(generalLookUp => generalLookUp.UniqueId == request.GeneralLookUp.UniqueId, cancellationToken)
                        ?? throw new NotFoundException("GeneralLookUp", request.GeneralLookUp.UniqueId);

            if (string.IsNullOrWhiteSpace(request.GeneralLookUp.Value)) throw new BadRequestException("Value field not available");

            bool isDuplicate = await _context.GeneralLookUps
                .AnyAsync(generalLookUp => generalLookUp.UniqueId != request.GeneralLookUp.UniqueId &&
                                generalLookUp.Value!.ToLower() == request.GeneralLookUp.Value!.ToLower() &&
                                generalLookUp.Type!.ToLower() == request.GeneralLookUp.Type.ToLower(), cancellationToken);

            if (isDuplicate) throw new AlreadyExistsException($"Type & Value", $"{request.GeneralLookUp.Type} & {request.GeneralLookUp.Value}");

            generalLookUp = _mapper.Map(request.GeneralLookUp, generalLookUp);

            await _context.SaveChangesAsync(cancellationToken);

        }
        catch (NotFoundException nf)
        {
            errors.Add(nf.Message);
        }
        catch (BadRequestException br)
        {
            errors.Add(br.Message);
        }
        catch (AlreadyExistsException ae)
        {
            errors.Add(ae.Message);
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
        }

        return errors.Any() ? Result.Failure(errors) : Result.Success();

    }
    #endregion
}
