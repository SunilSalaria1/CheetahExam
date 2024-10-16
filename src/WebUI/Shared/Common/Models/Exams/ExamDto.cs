using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheetahExam.WebUI.Shared.Common.Models.Exams;

public class ExamDto
{
    [DefaultValue("")]
    public string Name { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? ExamDate { get; set; }

    public TimeSpan? ExamDuration { get; set; }

    public int? AllowableAttempts { get; set; }

    public int? Category_GeneralLookUpID { get; set; }

    public int? Discipline_GeneralLookUpID { get; set; }

    public int? Result_GeneralLookUpID { get; set; }

    public double? PassingScore { get; set; }
    
    [DefaultValue(false)]
    public bool MarkForReview { get; set; } = false;

    public bool ISActive { get; set; } = true;

    [DefaultValue(false)]
    public bool ISArchive { get; set; }

    [DefaultValue("")]
    public string? FontStyle { get; set; } 
    
    [DefaultValue("")]
    public string? EncryptExamLink { get; set; }

    [DefaultValue("")]
    public string? UniqueId { get; set; }

    public int? QuestionCount { get; set; }

    public virtual MediaDto? Media { get; set; }

    public virtual GeneralLookUpDto? Category_GeneralLookUp { get; set; }

    public virtual GeneralLookUpDto? Discipline_GeneralLookUp { get; set; }

    public virtual GeneralLookUpDto? Result_GeneralLookUp { get; set; }

    public virtual List<QuestionDto> Questions { get; set; } = new();

    public virtual List<ExamResultOptionDto> ExamResultOptions { get; set; } = new();
}
