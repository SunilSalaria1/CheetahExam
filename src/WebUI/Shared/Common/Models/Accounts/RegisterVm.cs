using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CheetahExam.WebUI.Shared.Common.Models.Accounts;

public class RegisterVm
{
    public string? UniqueId { get; set; }

    [Required(ErrorMessage = "Please enter your first name.")]
    [Display(Name = "First Name")]
    public required string FirstName { get; set; }

    public string? LastName { get; set; }

    [Required(ErrorMessage = "Please provide an email address.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    [Display(Name = "Email")]
    public required string Email { get; set; } 
    [Required(ErrorMessage = "Please provide a password.")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public required string Password { get; set; } 
    [Required(ErrorMessage = "Please provide a confirm password.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public required string ConfirmPassword { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CompanyId { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    [Column(TypeName = "Date")]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }
}
