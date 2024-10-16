using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class Company : BaseAuditableEntity
{
    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? SupportContact { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public virtual Media? Media { get; set; }

    public virtual IList<User> Users { get; set; } = new List<User>();
}
