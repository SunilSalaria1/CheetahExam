using CheetahExam.WebUI.Shared.Common.Models.Exams;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static CheetahExam.WebUI.Shared.Enums.Enum;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions;

public partial class ChooseQuestionTypeDialog
{
    #region Fields

    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Parameter] public string ExamId { get; set; } = null!;

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = null!;

    private int SelectedType { get; set; } = QuestionTypes.CheckBox;

    #endregion

    #region Methods

    void Cancel() => MudDialog.Cancel();

    private void Select() { MudDialog.Close(SelectedType); }

    public void ChooseType(int id) { SelectedType = id; }

    public async void ImportQuestions()
    {
        DialogOptions options = new()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            DisableBackdropClick = true
        };

        DialogParameters parameters = new() { ["ExamId"] = ExamId };

        var result = await DialogService.Show<ImportQuestionDialog>("Import Questions From:", parameters, options).Result;

        if (result is not null)
        {
            if (result.Data is not null && result.Data.Equals(QuestionTypes.ImportQuestions)) { MudDialog.Close(result.Data); }
            else if (result.Canceled) { /*MudDialog.Cancel();*/ }
        }
    }

    private void HandleDragDropResult(int result) { MudDialog.Close(result); }

    #region Helpers

    /// <summary>
    /// We will get this from the server --------------------------------------->
    /// </summary>
    public List<QuestionType> Types = new()
    {
        new QuestionType(){ Index = "A", Id = 1, Type = "Check Box", Description = "Allows you to provide multiple possible answers in which only one is correct.", Icon = @Icons.Material.Filled.CheckBox},
        new QuestionType(){ Index = "B", Id = 2, Type = "Radio Buttons", Description = "Allows you to provide multiple answers that are correct.", Icon = @Icons.Material.Filled.RadioButtonUnchecked},
        new QuestionType(){ Index = "C", Id = 3, Type = "True False", Description = "Allows you to test a yes/no or true/false statement.", Icon = @Icons.Material.Filled.Check},
        new QuestionType(){ Index = "D", Id = 4, Type = "Fill in the Blanks", Description = "Allows you to leave space for a short text-based answer.", Icon = @Icons.Material.Filled.HorizontalRule},
        new QuestionType(){ Index = "E", Id = 5, Type = "Drop Down", Description = "Allows you to provide multiple answers in which only one is correct.", Icon = @Icons.Material.Filled.ArrowDropDownCircle},
        new QuestionType(){ Index = "F", Id = 6, Type = "Essay", Description = "Allows you to ask for a detailed text-based answer.", Icon = @Icons.Material.Filled.Assignment},
        new QuestionType(){ Index = "G", Id = 7, Type = "Order List", Description = "Allows you to provide a set of responses that can be arranged in a specified order.", Icon = @Icons.Material.Filled.FormatListNumbered},
        new QuestionType(){ Index = "H", Id = 8, Type = "Note", Description = "Allows you to leave a note for quiz takers.", Icon = @Icons.Material.Filled.Comment},
        new QuestionType(){ Index = "I", Id = 9, Type = "Upload Audio/Video", Description = "Allows you to upload an audio/video file to your quiz.", Icon = @Icons.Material.Filled.CloudUpload},
        new QuestionType(){ Index = "J", Id = 10, Type = "Comprehension", Description = "Allows you to create a questionaries for exam.", Icon = @Icons.Material.Filled.LibraryBooks},
        new QuestionType(){ Index = "k", Id = 11, Type = "Document", Description = "Allows you to provide an option to upload a document or image.", Icon = @Icons.Material.Filled.Description},
        new QuestionType(){ Index = "l", Id = 12, Type = "Record Audio/Video", Description = "Allows you to collect responses in a recorded audio/video format.", Icon = @Icons.Material.Filled.VideocamOff},
        new QuestionType(){ Index = "M", Id = 13, Type = "Matching", Description = "Allows you to create a question with Matching for exam.", Icon = @Icons.Material.Filled.CompareArrows},
        new QuestionType(){ Index = "N", Id = 14, Type = "Drag & Drop", Description = "Allows quiz takers to drag their responses from a list of options set by you, into empty response boxes.", Icon = @Icons.Material.Filled.DragHandle},
        //new QuestionType(){ Index = "O", Id = 15, Type = "DragDropWithMatching", Description = "Allows quiz takers to drag their responses from a list of options set by you, into empty response boxes."},
        new QuestionType(){ Index = "P", Id = 16, Type = "Hotspot", Description = "Allows you to show an image and set up hotspots to indicate the correct answer.", Icon = @Icons.Material.Filled.Brightness1}
    };

    #endregion

    #endregion
}
