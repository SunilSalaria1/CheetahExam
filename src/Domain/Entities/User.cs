using CheetahExam.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheetahExam.Domain.Entities;

public class User : BaseAuditableEntity
{
    public int? User_CompanyID { get; set; }

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string Email { get; set; } = null!;

    public bool IsEmailConfirmed { get; set; }

    public string? HashedPassword { get; set; }

    public string? PhoneNumber { get; set; }

    public bool IsPhoneNumberConfirmed { get; set; }

    public int AccessFailedCount { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? DateOfBirth { get; set; }

    public virtual Company? User_Company { get; set; }

    public virtual IList<UserRoleMapper> UserRoleMappers { get; set; } = new List<UserRoleMapper>();
}
