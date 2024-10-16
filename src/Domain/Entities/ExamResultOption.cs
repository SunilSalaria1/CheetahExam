using CheetahExam.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheetahExam.Domain.Entities;
public class ExamResultOption : BaseAuditableEntity
{
    public required int ExamResultOption_ExamID { get; set; }

    public required string OptionName { get; set; }

    public required double MinPercentage { get; set; }

    public required virtual Exam ExamResultOption_Exam { get; set; }
}
