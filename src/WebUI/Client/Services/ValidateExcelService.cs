using OfficeOpenXml;

namespace CheetahExam.WebUI.Client.Services;

public class ValidateExcelService : IValidateExcelService
{
    public async Task<List<string>> Validate(ExcelWorksheet workSheet)
    {
        int rowCount = workSheet.Dimension.End.Row;

        List<string> errors = new();

        var examName = workSheet.Cells[1, 2].Value;
        var examScore = workSheet.Cells[3, 2].Value;

        if (examName is null) { errors.Add($"|| Exam Name is required || Error reading value at index[row: {1}, col: {2}] ||"); }
        if (examScore is null) { errors.Add($"|| Exam Score is required || Error reading value at index[row: {3}, col: {2}] ||"); }

        for (int row = 7; row <= rowCount; row++)
        {
            var questionType = workSheet.Cells[row, 1].Value;
            var questionName = workSheet.Cells[row, 2].Value;

            if (questionType is null && questionName is null) { break; }
            if (questionType is not null && questionName is null) { break; }

            if (questionType is null) { errors.Add($"|| Question Type is required || Error reading value at index[row: {row}, col: {1}] ||"); }
            if (questionName is null) { errors.Add($"|| Question Name is required || Error reading value at index[row: {row}, col: {2}] ||"); }

        }

        return await Task.FromResult(errors);
    }
}
