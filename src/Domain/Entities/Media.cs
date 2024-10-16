using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class Media : BaseAuditableEntity
{
    public int? Media_QuestionID { get; set; }

    public int? Media_OptionID { get; set; }

    public int? MediaType_GeneralLookUpID { get; set; }

    public string? Url { get; set; }

    public virtual Question? Media_Question { get; set; }

    public virtual Option? Media_Option { get; set; }
}
