namespace CheetahExam.WebUI.Shared.Common.Models.Users;

public class RoleDto
{
    public RoleDto()
    {
        UniqueId = string.Empty;
        Name = string.Empty;
        //Permissions = Permissions.None;
    }

    public RoleDto(string uniqueId, string name)// , Permissions permissions)
    {
        UniqueId = uniqueId;
        Name = name;
        //Permissions = permissions;
    }

    public string UniqueId { get; set; }

    public string Name { get; set; }

    //public Permissions Permissions { get; set; }

    //public bool Has(Permissions permission)
    //{
    //    return Permissions.HasFlag(permission); ;
    //}

    //public void Set(Permissions permission, bool granted)
    //{
    //    if (granted)
    //    {
    //        Grant(permission);
    //    }
    //    else
    //    {
    //        Revoke(permission);
    //    }
    //}

    //public void Grant(Permissions permission)
    //{
    //    Permissions |= permission;
    //}

    //public void Revoke(Permissions permission)
    //{
    //    Permissions ^= permission;
    //}
}
