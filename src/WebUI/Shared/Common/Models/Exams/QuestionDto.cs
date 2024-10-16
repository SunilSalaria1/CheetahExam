using System.ComponentModel;

namespace CheetahExam.WebUI.Shared.Common.Models.Exams;

public class QuestionDto
{
    [DefaultValue("")]
    public string? UniqueId { get; set; }

    public string? ExamUniqueId { get; set; }

    public int DisplayOrder { get; set; }

    public int? QuestionLevelType_GeneralLookUpID { get; set; }

    [DefaultValue("")]
    public string? Name { get; set; }
    
    [DefaultValue("")]
    public string? Description { get; set; }

    public int? QuestionType_GeneralLookUpID { get; set; }

    [DefaultValue("")]
    public string? ParentQuestionId { get; set; }

    public int? OldDisplayOrder { get; set; }

    public virtual List<OptionDto> Options { get; set; } = new();

    public virtual MediaDto? Media { get; set; }
}
