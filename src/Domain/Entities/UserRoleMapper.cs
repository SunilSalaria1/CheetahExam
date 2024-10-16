using CheetahExam.Domain.Common;

namespace CheetahExam.Domain.Entities;

public class UserRoleMapper : BaseAuditableEntity
{
    public required int? UserRoleMapper_UserID { get; set; }

    public required int? UserRoleMapper_RoleID { get; set; }

    public virtual User? UserRoleMapper_User { get; set; }

    public virtual Role? UserRoleMapper_Role { get; set; }
}
