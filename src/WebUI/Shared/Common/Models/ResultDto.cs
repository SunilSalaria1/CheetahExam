namespace CheetahExam.WebUI.Shared.Common.Models;

public class ResultDto
{
    public bool Succeeded { get; set; } = true;

    public IEnumerable<string>? Errors { get; set; }

    public string? UserId { get; set; }

    public string? Token { get; set; }

}
