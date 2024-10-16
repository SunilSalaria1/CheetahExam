using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class Font : BaseAuditableEntity
{
    public string Name { get; set; } = null!;

    public string? Family { get; set; }
}
