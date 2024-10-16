using CheetahExam.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheetahExam.Domain.Entities;

public class Exam : BaseAuditableEntity
{
    public required string Name { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ExamDate { get; set; }

    public TimeSpan? ExamDuration { get; set; }

    public int? AllowableAttempts { get; set; }

    public int? Category_GeneralLookUpID { get; set; }

    public int? Discipline_GeneralLookUpID { get; set; }

    public int? Result_GeneralLookUpID { get; set; }

    public bool MarkForReview { get; set; }

    public string? FontStyle { get; set; }

    public string? EncryptExamLink { get; set; }

    public double? PassingScore { get; set; }

    public virtual IList<Question> Questions { get; set; } = new List<Question>();

    public virtual Media? Media { get; set; }

    public virtual GeneralLookUp? Category_GeneralLookUp { get; set; }

    public virtual GeneralLookUp? Discipline_GeneralLookUp { get; set; }

    public virtual GeneralLookUp? Result_GeneralLookUp { get; set; }

    public virtual IList<ExamResultOption> ExamResultOptions { get; set; } = new List<ExamResultOption>();
}
