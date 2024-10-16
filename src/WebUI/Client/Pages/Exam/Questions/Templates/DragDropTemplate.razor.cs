using Microsoft.AspNetCore.Components;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions.Templates;

public partial class DragDropTemplate
{
    [Parameter]
    public EventCallback<int> SelectedOption { get; set; }

    private int UserChoose { get; set; } = 0;
}
