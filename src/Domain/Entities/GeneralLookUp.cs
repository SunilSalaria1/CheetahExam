using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class GeneralLookUp : BaseAuditableEntity
{
    public string Type { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }
}
