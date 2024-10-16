using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class EssayTemplate
{
    QuestionDto Question { get; set; } = null!;

    MudForm Form { get; set; } = null!;

    protected override void OnInitialized()
    {
        Question = new()
        {
            Name = "Describe in 200 words the most influential person in your life and why.",
            Options = new() { new() { Name = "",} }
        };
    }
}
