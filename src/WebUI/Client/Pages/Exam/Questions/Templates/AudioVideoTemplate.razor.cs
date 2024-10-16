using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class AudioVideoTemplate
{
    public MudForm Form { get; set; }

    public QuestionDto Model { get; set; } = new();

    protected override void OnInitialized()
    {
        Model.Name = "Listen/watch the audio/video clip and answer the following questions.";
    }
}
