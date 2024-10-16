using OfficeOpenXml;

namespace CheetahExam.WebUI.Client.Services;

public interface IValidateExcelService
{
    Task<List<string>> Validate(ExcelWorksheet workSheet);
}
