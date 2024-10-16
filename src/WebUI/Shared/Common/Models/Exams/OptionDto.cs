using System.ComponentModel;

namespace CheetahExam.WebUI.Shared.Common.Models.Exams;

public class OptionDto
{
    [DefaultValue("")]
    public string? UniqueId { get; set; }

    public int DisplayOrder { get; set; }

    [DefaultValue("")]
    public string? Name { get; set; }

    [DefaultValue("")]
    public string? Match { get; set; }

    public int? Index { get; set; }

    public MediaDto? Media { get; set; }

    [DefaultValue(false)]
    public bool ISInCorrectMatch { get; set; }

    [DefaultValue(false)]
    public bool ISCorrect { get; set; }
}
