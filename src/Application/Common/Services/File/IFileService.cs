using CheetahExam.WebUI.Shared.Common.Models;

namespace CheetahExam.Application.Common.Services.File;

public interface IFileService
{
    Task<MediaDto> UploadImage(FileDetailDto fileDetailDto);

    Task<MemoryStream> ExportExam(string examId);
}
