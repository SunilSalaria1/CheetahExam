using System.ComponentModel;

namespace CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;

public class GeneralLookUpDto
{
    [DefaultValue("")]
    public string? UniqueId { get; set; }

    [DefaultValue("")]
    public string Type { get; set; } = null!;

    [DisplayName("Name")]
    [DefaultValue("")]
    public string? Value { get; set; }

    [DefaultValue("")]
    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }

    public DateTime CreatedUtc { get; set; }

    public bool ISActive { get; set; } = true;

    public bool ISArchive { get; set; }

}
