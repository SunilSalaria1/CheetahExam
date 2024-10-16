using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class RadioButtonTemplate
{
    public QuestionDto Question { get; set; } = null!;

    int SelectedOption { get; set; }

    private MudForm Form { get; set; } = null!;
    protected override void OnInitialized()
    {
        Question = new()
        {
            Name = "Reuters is the News Agency of which country?",
            Options = new()
            {
                new() { Name = "United Kingdom" },
                new() { Name = "France" },
                new() { Name = "Germany" },
                new() { Name = "Italy" }
            }
        };
    }
}
