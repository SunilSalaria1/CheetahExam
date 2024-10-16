using Microsoft.EntityFrameworkCore;

namespace CheetahExam.WebUI.Shared.Constants;

public static class Constant
{
    public static class Common
    {
        [Comment("Default header for tabs while loading the data for general lookups tabs")]
        public const string DefaultHeader = "Default Header";

        [Comment("Default value for General lookup type list")]
        public const string Loading = "Loading";

        [Comment("Default value for dropdown lists")]
        public const string DefaultDropdownValue = "All";
    }

    public static class LookUpTypes
    {
        public const string Discipline_Type = "Discipline_Type";

        public const string Media_Type = "Media_Type";

        public const string Question_Type = "Question_Type";

        public const string QuestionLevel_Type = "QuestionLevel_Type";

        public const string Category_Type = "Category_Type";

        public const string Result_Type = "Result_Type";
    }

    public class CommandsReturnStatus
    {
        public const string Created = "Created";

        public const string Updated = "Updated";

        public const string Deleted = "Deleted";

        public const string Retrieved = "Retrieved";

        public const string Exception = "Exception";

        public const string AlreadyExist = "AlreadyExist";

        public const string NotFound = "NotFound";
    }

    public class FileDirectory
    {
        public const string ExamImage = "images/exam/";

        public const string QuestionImage = "images/question/";

        public const string OptionImage = "images/option/";

        public const string DefaultImage = "/images/defaultImage";

        public const string CompanyImage = "images/company";

        public const string SampleExcel = "excels/2024 exam template for new system.xlsx";

        public const string ExamTemplate = "2024 Exam Template for New System.xlsx";
    }

    public class FileTypes
    {
        public const string JPEG = "image/jpeg";

        public const string PNG = "image/png";

        public const string GIF = "image/gif";

        public const string JPG = "image/jpg";
    }

    public class ResultType
    {
        public const string PassFail = "Pass or Fail";

        public const string LetterGrading = "Letter Grading";

        public const string GoodExcellent = "Good or Excellent";
    }

    public class UserRoles
    {
        public const string Administrator = "Administrator";

        public const string SuperAdmin = "Super Admin";

        public const string Admin = "Admin";

        public const string Instructor = "Instructor";

        public const string Students = "Students";
    }
}
