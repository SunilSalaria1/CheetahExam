using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using CheetahExam.WebUI.Shared.Utility;
using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;

namespace CheetahExam.WebUI.Client.Services;

public class ExamImportExportService : IExamImportExportService
{
    #region Fields

    private readonly IValidateExcelService _validateExcelService;

    #endregion

    #region Ctor

    public ExamImportExportService(
        IValidateExcelService validateExcelService)
    {
        _validateExcelService = validateExcelService;
    }

    #endregion

    #region Methods

    public async Task<Result<ExamDto>> GetExam(IBrowserFile file)
    {
        ExamDto exam = new();
        List<string> errors = new();

        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(ms);
                ms.Position = 0;

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(ms))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    errors = await _validateExcelService.Validate(ws);

                    if (errors.Any()) { return Result<ExamDto>.Failure(errors); }

                    exam = await GetExam(ws);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        return Result<ExamDto>.Success(exam);

    }

    #region Helpers

    private async Task<ExamDto> GetExam(ExcelWorksheet workSheet)
    {
        List<string> errors = new();

        ExamDto exam = new();

        int rowCount = workSheet.Dimension.End.Row;

        var passingScore = workSheet.Cells[3, 2].Value?.ToString();

        exam.Name = workSheet.Cells[1, 2].Value?.ToString() ?? "";
        exam.PassingScore = double.TryParse(passingScore, out double parsedValue) ? parsedValue : null;

        // Loop through the rows and columns to get the questions and options
        for (int row = 7; row <= rowCount; row++)
        {
            var questionType = workSheet.Cells[row, 1].Value;
            var questionName = workSheet.Cells[row, 2].Value;

            // If the question type or question name is null, then break the loop because we have reached the end of the questions.
            if (questionType is null || questionName is null) { break; }

            QuestionDto question = new();
            List<OptionDto> options = new();

            for (int col = 1; col <= 14; col++)
            {
                // Get the value of the cell
                var value = workSheet.Cells[row, col].Value?.ToString();

                if (value is not null)
                {
                    switch (col)
                    {
                        case 1:
                            question.QuestionType_GeneralLookUpID = CommonHelper.GetQuestionTypeFromValue(value);
                            question.DisplayOrder = exam.Questions.Any() ? exam.Questions.Max(question => question.DisplayOrder) + 1 : 1;
                            break;
                        case 2:
                            question.Name = value;
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            options.Add(new OptionDto() { Name = value, DisplayOrder = col - 3 });
                            break;
                        case 8:
                            // Set the correct answer here
                            CommonHelper.SetCorrectOption(options, value);
                            break;
                        default:
                            break;
                    }
                }
            }
            question.Options = options;
            exam.Questions.Add(question);
        }

        return exam;
    }

    #endregion

    #endregion
}
