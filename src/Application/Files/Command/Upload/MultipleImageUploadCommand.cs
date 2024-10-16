using CheetahExam.Application.Common.Services.File;
using CheetahExam.WebUI.Shared.Common.Models;


namespace CheetahExam.Application.Files.Command.Upload;

public record MultipleImageUploadCommand : IRequest<MediaCollectionDto>
{
    public FileDetailCollectionDto FileDetailCollection { get; init; } = null!;
}

public class MultipleImageUploadCommandHandler : IRequestHandler<MultipleImageUploadCommand, MediaCollectionDto>
{

    #region Fields

    private readonly IFileService _fileService;

    #endregion

    #region Ctor

    public MultipleImageUploadCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    #endregion

    #region Method

    public async Task<MediaCollectionDto> Handle(MultipleImageUploadCommand request, CancellationToken cancellationToken)
    {
        MediaCollectionDto media = new();

        foreach (var fileDetail in request.FileDetailCollection.QuestionOptionsFile)
        {
            if (fileDetail is not null) 
            {
                var file = await _fileService.UploadImage(fileDetail);

                media.QuestionOptionMediaDto.Add(new () 
                { 
                    MediaType_GeneralLookUpID = file.MediaType_GeneralLookUpID,
                    Url = file.Url,
                    UniqueId = file.UniqueId
                });
            }
        }

        return media;
    }

    #endregion
}
