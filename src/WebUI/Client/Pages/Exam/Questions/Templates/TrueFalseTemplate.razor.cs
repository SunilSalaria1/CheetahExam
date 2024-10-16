using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class TrueFalseTemplate
{
    public QuestionDto Question { get; set; } = null!;

    int SelectedOption { get; set; }

    protected override void OnInitialized()
    {
        Question = new()
        {
            Name = "Satisfied customers are usually loyal customers.",
            Options = new()
            {
                new() { Name = "True"},
                new() { Name = "False",}
            }
        };
    }
}
