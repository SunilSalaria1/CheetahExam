namespace CheetahExam.WebUI.Shared.Common.Models.Users;

public class UserDto
{
    public string? UniqueId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool ISActive { get; set; } = true;

    public List<string> Roles { get; set; } = new();
}
