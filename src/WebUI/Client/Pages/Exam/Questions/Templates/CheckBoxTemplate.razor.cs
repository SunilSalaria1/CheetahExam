using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class CheckBoxTemplate
{
    public QuestionDto Question { get; set; } = null!;

    protected override void OnInitialized()
    {
        Question = new()
        {
            Name = "Good customer service representatives always (choose all answers that apply)",
            Options = new()
            {
                new() { Name = "Listen attentively", },
                new() { Name = "Use lots of technical terms", },
                new() { Name = "Maintain a positive attitude", },
                new() { Name = "Over-promise and under-deliver", }
            }
        };
    }
}
