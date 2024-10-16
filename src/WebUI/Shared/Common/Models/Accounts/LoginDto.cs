using System.ComponentModel.DataAnnotations;

namespace CheetahExam.WebUI.Shared.Common.Models.Accounts;

public class LoginDto
{
    [Required(ErrorMessage = "Please provide an email address")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide a password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
