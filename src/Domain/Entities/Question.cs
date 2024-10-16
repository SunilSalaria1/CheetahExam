using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class Question : BaseAuditableEntity
{
    public int? Question_ExamID { get; set; }

    public int? QuestionLevelType_GeneralLookUpID { get; set; }

    public required int DisplayOrder { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? QuestionType_GeneralLookUpID { get; set; }

    public string? ParentQuestionId { get; set; }

    public int? OldDisplayOrder { get; set; }

    public virtual IList<Option> Options { get; set; } = new List<Option>();

    public virtual Media? Media { get; set; }

    public virtual Exam? Question_Exam { get; set; }
}
