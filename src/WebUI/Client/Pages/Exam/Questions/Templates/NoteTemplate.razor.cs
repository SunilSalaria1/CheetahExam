using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class NoteTemplate
{
    public MudForm Form { get; set; }

    public QuestionDto Model { get; set; } = new();

    protected override void OnInitialized()
    {
        Model.Name = "The following questions are going to get harder.";
    }
}
