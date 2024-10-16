using CheetahExam.WebUI.Client.Pages.CommonComponents;
using CheetahExam.WebUI.Client.Services;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using static CheetahExam.WebUI.Shared.Enums.Enum;

namespace CheetahExam.WebUI.Client.Pages.Exam.Questions;

public partial class ImportQuestionDialog
{
    #region Fields

    [Parameter] public string? ExamId { get; set; }

    [Inject] ISnackbar SnackBar { get; set; } = null!;

    [Inject] IExamsClient ExamsClient { get; set; } = null!;

    [Inject] IExamImportExportService ExamImportExportService { get; set; } = null!;

    [Inject] IQuestionsClient QuestionsClient { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    public string SelectedFileName { get; set; } = string.Empty;

    public List<ExamDto> Exams { get; set; } = new();

    public List<QuestionDto> Questions { get; set; } = new();

    private List<QuestionDto> SelectedQuestions = new();

    IBrowserFile ImportedFile { get; set; } = null!;

    private bool IsExamSelected { get; set; }

    public string Message { get; set; } = string.Empty;

    MudBlazor.Severity Severity { get; set; }

    List<string> Errors { get; set; } = new();

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrWhiteSpace(ExamId))
        {
            await GetExams();
        }
    }

    public async Task GetExams()
    {
        var result = await ExamsClient.GetAllAsync(null, null, null);

        Exams = result.Exams.Where(exam => exam.UniqueId != ExamId).ToList();
    }

    public async void Import()
    {
        if (string.IsNullOrWhiteSpace(ExamId) && ImportedFile is not null)
        {
            var result = await ExamImportExportService.GetExam(ImportedFile);

            if (result.Succeeded)
            {
                if (result.Value is not null)
                {
                    var examId = await ExamsClient.CreateAsync(result.Value);

                    Severity = MudBlazor.Severity.Success;
                    Message = "Exam imported successfully";

                    MudDialog.Close(SnackBar.Add(Message, Severity));
                }
            }
            else
            {
                DialogOptions options = new() { CloseButton = true, MaxWidth = MaxWidth.Medium};

                DialogParameters parameters = new() { ["Errors"] = result.Errors.ToList(), ["Title"] = "Error Reading Excel"};

                var dialogResult = DialogService.ShowAsync<ErrorDialog>($"There are some Errors in Excel{result.Errors.Count()}", parameters, options).Result;
            }
        }
        if (IsExamSelected && Questions.Any())
        {
            Questions = new List<QuestionDto>();

            Questions.AddRange(SelectedQuestions);

            var highestDisplayOrder = await QuestionsClient.GetLargestDisplayOrderAsync(ExamId);

            var newDisplayOrder = highestDisplayOrder is 0 ? 1 : highestDisplayOrder + 1;

            Questions = Questions.Select(question =>
            {
                question.ExamUniqueId = ExamId;
                question.UniqueId = string.Empty;
                question.DisplayOrder = newDisplayOrder++;
                question.Media = question.Media is not null ? new MediaDto()
                {
                    MediaType_GeneralLookUpID = question.Media.MediaType_GeneralLookUpID,
                    UniqueId = string.Empty,
                    Url = question.Media.Url
                } : new MediaDto();
                question.Options = question.Options.Select(option =>
                {
                    option.UniqueId = string.Empty;
                    option.Media = option.Media is not null ? new MediaDto()
                    {
                        MediaType_GeneralLookUpID = option.Media.MediaType_GeneralLookUpID,
                        UniqueId = string.Empty,
                        Url = option.Media.Url
                    } : new MediaDto();
                    return option;
                }).ToList();

                return question;
            }).ToList();

            QuestionCollection questionCollection = new() { Questions = Questions };

            var result = await QuestionsClient.CreateMultipleAsync(examId: ExamId, questionCollection: questionCollection);

            if (result)
            {
                Severity = MudBlazor.Severity.Success;
                Message = "Question imported successfully";

                MudDialog.Close(QuestionTypes.ImportQuestions);
            }
            else
            {
                Severity = MudBlazor.Severity.Success;
                Message = "Question import Failed!";
            }

            MudDialog.Close(SnackBar.Add(message: Message, severity: Severity));
        }
    }

    public void Back() { IsExamSelected = false; }

    void Cancel() => MudDialog.Cancel();

    public async Task SelectedItemsChanged(HashSet<QuestionDto> selectedQuestions)
    {
        SelectedQuestions = selectedQuestions.ToList();
    }

    private async Task HandleExamChange(string exam)
    {
        var examName = exam.Substring(0, exam.LastIndexOf("(")).Trim();

        var selectedExamId = Exams.FirstOrDefault(x => x.Name.Trim() == examName.Trim()) ?? new ExamDto();

        var result = await QuestionsClient.GetAllByExamIdAsync(selectedExamId.UniqueId);

        Questions = result.Questions.ToList();

        IsExamSelected = true;

        StateHasChanged();
    }

    public async Task ImportExam(InputFileChangeEventArgs file)
    {
        string extension = Path.GetExtension(file.File.Name);

        if (extension == ".xlsx")
        {
            SelectedFileName = file.File.Name;

            ImportedFile = file.File;
        }
        else
        {
            Severity = MudBlazor.Severity.Error;
            Message = "Invalid file format!! We only support '.xlxs' format.";
        }

        SnackBar.Add(message: Message, severity: Severity);
    }

    public async void ToggleError()
    {
        Errors.Clear();
    }
    #endregion
}
