using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class Role : BaseAuditableEntity
{
    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public virtual IList<UserRoleMapper> UserRoleMappers { get; set; } = new List<UserRoleMapper>();
}
