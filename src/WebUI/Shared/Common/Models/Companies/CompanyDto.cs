using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CheetahExam.WebUI.Shared.Common.Models.Companies;

public class CompanyDto
{
    [DefaultValue("")]
    public string? UniqueId { get; set; }

    [DefaultValue("")]
    public string Name { get; set; } = null!;

    [DefaultValue("")]
    public string? PhoneNumber { get; set; }

    [DefaultValue("")]
    public string? SupportContact { get; set; }

    [DefaultValue("")]
    public string? City { get; set; }

    [DefaultValue("")]
    public string? State { get; set; }

    [DefaultValue("")]
    public string? Country { get; set; }

    [DefaultValue("")]
    public string? PostalCode { get; set; }

    [DefaultValue("")]
    public string? Address { get; set; }

    public bool ISActive { get; set; } = true;

    public virtual MediaDto? Media { get; set; }
}
