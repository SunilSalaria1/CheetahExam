using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CheetahExam.WebUI.Shared.Common.Models;

public class ExamResultOptionDto
{
    [DefaultValue("")]
    public string? UniqueId { get; set; }

    [Required]
    public int ExamResultOption_ExamID { get; set; }

    [Required]
    public string OptionName { get; set; }

    [Required]
    [DefaultValue("")]
    public double MinPercentage { get; set; }
}

