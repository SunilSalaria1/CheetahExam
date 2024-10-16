using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class FillBlanksTemplate
{
    QuestionDto Question { get; set; } = null!;

    string Answer { get; set; } = null!;

    private MudForm Form { get; set; } = null!;

    protected override void OnInitialized()
    {
        Question = new()
        {
            Name = "The earth _____ around the sun.",
            Options = new(){ new() { Name = "" } }
        };
    }
}
