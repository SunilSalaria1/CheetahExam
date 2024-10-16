using CheetahExam.Application.Common.Services.File;
using CheetahExam.WebUI.Shared.Common.Models;

namespace CheetahExam.Application.Files.Command.Upload;

public record ImageUploadCommand : IRequest<MediaDto>
{
    public FileDetailDto FileDetail { get; init; } = null!;
}

public class ImageUploadCommandHandler : IRequestHandler<ImageUploadCommand, MediaDto>
{
    #region Fields

    private readonly IFileService _fileService;

    #endregion

    #region Ctor

    public ImageUploadCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    #endregion

    #region Method

    public async Task<MediaDto> Handle(ImageUploadCommand request, CancellationToken cancellationToken)
    {
        return await _fileService.UploadImage(request.FileDetail);
    }

    #endregion
}
