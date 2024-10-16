using System.ComponentModel;

namespace CheetahExam.WebUI.Shared.Common.Models;

public class MediaDto
{
    [DefaultValue("")]
    public string? UniqueId { get; set; }

    public int? MediaType_GeneralLookUpID { get; set; }

    [DefaultValue("")]
    public string? Url { get; set; }
}
