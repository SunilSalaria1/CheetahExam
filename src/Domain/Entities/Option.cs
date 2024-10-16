using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class Option : BaseAuditableEntity
{
    public int? Option_QuestionID { get; set; }

    public required int DisplayOrder { get; set; }

    public string Name { get; set; } = null!;

    public string? Match { get; set; }

    public string? Description { get; set; }

    public bool ISCorrect { get; set; }

    public bool ISInCorrectMatch { get; set; }

    public virtual Media? Media { get; set; }

    public virtual Question? Option_Question { get; set; }
}
