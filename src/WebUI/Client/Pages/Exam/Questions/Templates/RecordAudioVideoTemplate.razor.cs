using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class RecordAudioVideoTemplate
{
    public MudForm Form { get; set; }

    public QuestionDto Model { get; set; } = new();

    protected override void OnInitialized()
    {
        Model.Name = "Record your response.";
        Model.Media = new MediaDto
        {
            Url = "/images/recordaudiovideio.png"
        };
    }
}
