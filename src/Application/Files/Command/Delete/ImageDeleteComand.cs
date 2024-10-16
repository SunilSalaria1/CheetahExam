using static CheetahExam.WebUI.Shared.Constants.Constant;

namespace CheetahExam.Application.Files.Command.Delete;

public record ImageDeleteComand : IRequest<string>
{
    public required string FilePath { get; init; }

}

public class ImageDeleteComandHandler : IRequestHandler<ImageDeleteComand, string>
{
    #region Methods

    public async Task<string> Handle(ImageDeleteComand request, CancellationToken cancellationToken)
    {

        var filePath = "wwwroot/" + request.FilePath;

        if (File.Exists(filePath))
        {
            File.Delete(filePath);

            return await Task.FromResult(CommandsReturnStatus.Deleted);
        }

        return await Task.FromResult(CommandsReturnStatus.NotFound);
    }

    #endregion
}
