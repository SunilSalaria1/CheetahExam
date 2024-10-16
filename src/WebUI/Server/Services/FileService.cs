using CheetahExam.Application.Common.Services.Data;
using CheetahExam.Application.Common.Services.File;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Constants;
using CheetahExam.WebUI.Shared.Utility;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace CheetahExam.WebUI.Server.Services;

public class FileService : IFileService
{
    #region Fields

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IApplicationDbContext _context;

    #endregion

    #region Ctor

    public FileService(IWebHostEnvironment webHostEnvironment,
        IApplicationDbContext context)
    {
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<MediaDto> UploadImage(FileDetailDto fileDetail)
    {
        string fileExtension = Path.GetExtension(fileDetail.MetaData.FileName);

        string fileName = Path.GetFileNameWithoutExtension(fileDetail.MetaData.FileName);

        string filePath = fileDetail.Path + fileName + DateTime.UtcNow.Ticks.ToString() + fileExtension;

        using (var fileStream = File.Create(_webHostEnvironment.WebRootPath + "/" + filePath))
        {
            await fileStream.WriteAsync(fileDetail.MetaData.FileBytes);
        }

        return new MediaDto() { MediaType_GeneralLookUpID = 32, Url = filePath };
    }

    public async Task<MemoryStream> ExportExam(string examId)
    {
        try
        {
            // Get exam from the database including questions and options
            var exam = await _context.Exams
                .Where(e => e.UniqueId == examId)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync();

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "excel", Constant.FileDirectory.ExamTemplate);

            // Read the Excel template into a MemoryStream
            MemoryStream templateStream = new MemoryStream();
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(templateStream);
            }

            // Create ExcelPackage from the templateStream
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(templateStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                worksheet = await UpdateExcel(exam, worksheet);

                // Save the changes to a new MemoryStream
                MemoryStream modifiedStream = new MemoryStream(package.GetAsByteArray());

                modifiedStream.Position = 0;

                return modifiedStream;
            }
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            throw new Exception("An error occurred while exporting the exam", ex);
        }
    }

    private static async Task<ExcelWorksheet> UpdateExcel(Exam exam, ExcelWorksheet worksheet)
    {
        int startRow = 7;
        int optionColumn = 3;

        worksheet.Cells["B1"].Value = exam.Name;

        foreach (var question in exam.Questions.OrderBy(question => question.DisplayOrder))
        {
            worksheet.Cells[startRow, 1].Value = CommonHelper.GetQuestionTypeWithId(Convert.ToInt32(question.QuestionType_GeneralLookUpID));
            worksheet.Cells[startRow, 2].Value = question.Name;

            foreach (var option in question.Options.OrderBy(option => option.DisplayOrder))
            {
                if (optionColumn == 8) { break; }

                worksheet.Cells[startRow, optionColumn].Value = option.Name;

                if (option.ISCorrect) { worksheet.Cells[startRow, 8].Value = CommonHelper.GetAlphabetByIndex(option.DisplayOrder); }

                optionColumn++;
            }

            startRow++;
            optionColumn = 3;
        }

        return await Task.FromResult(worksheet);
    }

    #endregion
}
