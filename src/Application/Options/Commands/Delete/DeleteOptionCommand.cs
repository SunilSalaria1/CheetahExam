namespace CheetahExam.Application.Options.Commands.Delete;

public record DeleteOptionCommand : IRequest
{
    public string? OptionId { get; set; }
}

public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand>
{
    #region Fields

    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public DeleteOptionCommandHandler(
        IApplicationDbContext context)

    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<Unit> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
    {
        var option = await _context.Options.Where(option => option.UniqueId == request.OptionId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cancellationToken);

        if (option is not null)
        {
            //await _context.Media.Where(media => media.Media_OptionID == option.Id)
            //        .ForEachAsync(media => _context.Media.Remove(media), cancellationToken);
            var mediaList = await _context.Media
                .Where(media => media.Media_OptionID == option.Id)
                .ToListAsync(cancellationToken);

            mediaList.ForEach(media => _context.Media.Remove(media));


            _context.Options.Remove(option);

            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }

    #endregion
}
