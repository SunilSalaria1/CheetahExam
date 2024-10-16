namespace CheetahExam.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public bool ISArchive { get; set; }

    public bool ISActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime LastModifiedUtc { get; set; }
}
