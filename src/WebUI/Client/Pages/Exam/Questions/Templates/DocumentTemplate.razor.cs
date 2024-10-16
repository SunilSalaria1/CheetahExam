using CheetahExam.WebUI.Shared.Common.Models.Exams;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class DocumentTemplate
{
    public MudForm Form { get; set; }

    public QuestionDto Model { get; set; } = new();

    protected override void OnInitialized()
    {
        Model.Name = "Download and read the PDF file shared with you and answer the questions that follow.";
        Model.Description = "Type description here";
    }
}
