using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using Microsoft.AspNetCore.Components.Forms;

namespace CheetahExam.WebUI.Client.Services;

public interface IExamImportExportService
{
    Task<Result<ExamDto>> GetExam(IBrowserFile file);
}
