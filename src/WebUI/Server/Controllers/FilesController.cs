using CheetahExam.Application.Files.Command.Delete;
using CheetahExam.Application.Files.Command.Upload;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CheetahExam.WebUI.Server.Controllers
{
    [Route("/")]
    [Authorize(Roles = $"{Constant.UserRoles.Administrator},{Constant.UserRoles.SuperAdmin},{Constant.UserRoles.Admin}")]
    public class FilesController : ApiControllerBase
    {
        [HttpPost("files/upload")]
        [OpenApiOperation("Upload a File", "Upload a new Image File.")]
        public async Task<MediaDto> Upload(FileDetailDto fileDetail)
        {
            return await Mediator.Send(new ImageUploadCommand() { FileDetail = fileDetail });
        }

        [HttpPost("files/uploadMultiple")]
        [OpenApiOperation("Upload Multiple images", "Upload a collection of Files. ")]
        public async Task<MediaCollectionDto> UploadMultiple(FileDetailCollectionDto fileDetailCollection)
        {
            return await Mediator.Send(new MultipleImageUploadCommand() { FileDetailCollection = fileDetailCollection });
        }

        [HttpDelete("files/delete/{filePath}")]
        [OpenApiOperation("Delete an exisitng file", "Delete an exisitng file with file path.")]
        public async Task<string> Delete(string filePath)
        {
            return await Mediator.Send(new ImageDeleteComand() { FilePath = filePath });
        }
    }
}
