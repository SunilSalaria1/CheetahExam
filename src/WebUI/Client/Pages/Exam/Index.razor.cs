using CheetahExam.WebUI.Client.Pages.Exam.Questions;
using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using static CheetahExam.WebUI.Shared.Constants.Constant;

namespace CheetahExam.WebUI.Client.Pages.Exam;

public partial class Index : ComponentBase
{
    #region Fields

    #region Services

    [Inject] IExamsClient ExamsClient { get; set; } = null!;

    [Inject] IJSRuntime JSRuntimeClient { get; set; } = null!;

    [Inject] IGeneralLookUpsClient GeneralLookUpsClient { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    private bool ISLoading = true;

    private bool ISActive = false;

    private string? SearchString { get; set; }

    private ActiveExamCount ActiveExamsCount { get; set; } = new();

    private List<ExamDto> Exams { get; set; } = new();

    public const string DefaultImage = FileDirectory.DefaultImage;

    private List<KeyValue> DisciplineItems { get; set; } = new List<KeyValue>();

    private string? SelectedDiscipline { get; set; }

    private List<KeyValue> CategoryItems { get; set; } = new List<KeyValue>();

    private string? SelectedCategory { get; set; }

    private string? DialogImageURL { get; set; }

    private bool ISImageDialogVisible = false;

    private readonly DialogOptions ImageDialogOptions = new() { CloseButton = true, NoHeader = true, MaxWidth = MaxWidth.Medium };

    #endregion

    #region Methods

    protected async override Task OnInitializedAsync()
    {
        CategoryItems = (await GeneralLookUpsClient.GetByTypeAsync(LookUpTypes.Category_Type)).ToList();
        DisciplineItems = (await GeneralLookUpsClient.GetByTypeAsync(LookUpTypes.Discipline_Type)).ToList();

        await GetExams();
    }

    private async Task HandleCategoryValueChanged(string selectedCategory)
    {
        SelectedCategory = selectedCategory;
        await GetExams();
    }

    private async Task HandleDisciplineValueChanged(string selectedDiscipline)
    {
        SelectedDiscipline = selectedDiscipline;
        await GetExams();
    }

    private async Task HandleActiveToggle(bool isActive)
    {
        ISActive = isActive;
        await GetExams();
    }

    private async Task GetExams()
    {
        ISLoading = true;

        var exams = await ExamsClient.GetAllAsync(ISActive, Convert.ToInt32(SelectedCategory ?? "0"), Convert.ToInt32(SelectedDiscipline ?? "0"));

        Exams = ISActive ? exams.Exams.Where(exam => exam.ISActive).ToList() : exams.Exams;
        ActiveExamsCount = new ActiveExamCount
        {
            ActiveExams = exams.Exams.Count(exam => exam.ISActive),
            TotalExams = exams.Exams.Count()
        };

        ISLoading = false;
        StateHasChanged();
    }

    private async Task HandleExamDeleteAction(string? uniqueId, string dialogType)
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraLarge,
            NoHeader = true,
            Position = DialogPosition.Center,
        };

        var dialog = await DialogService.ShowAsync(typeof(Delete), "", new DialogParameters<Delete>
        {
            { x => x.UniqueId, uniqueId },
            { x => x.DialogType, dialogType }
        }, options);

        var result = await dialog.Result;

        if (!result.Canceled) { await GetExams(); }
    }

    private void AddExam()
    {
        NavigationManager.NavigateTo("/exam");
    }

    private void EditExam(ExamDto exam)
    {
        NavigationManager.NavigateTo("/exam/" + exam.UniqueId);
    }

    private void QuestionOptions(ExamDto exam)
    {
        NavigationManager.NavigateTo("/exams/questions/" + exam.UniqueId);
    }

    private async void ExportExam(ExamDto exam) 
    {
        try
        {
            var result = await ExamsClient.ExportAsync(exam.UniqueId);

            using (var memoryStream = new MemoryStream())
            {
                await result.Stream.CopyToAsync(memoryStream);

                byte[] fileBytes = memoryStream.ToArray();

                var fileName = $"{exam.Name}_{DateTime.UtcNow.Ticks.ToString()}.xlsx";

                await JSRuntimeClient.InvokeVoidAsync("DownloadExamInExcel", fileName, fileBytes);
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void PopupImage(string imageUrl)
    {
        DialogImageURL = imageUrl;
        ISImageDialogVisible = true;

        StateHasChanged();
    }

    private void ClosePopupImage() => ISImageDialogVisible = false;

    private Func<ExamDto, bool> QuickFilter => x =>
    {
        return string.IsNullOrWhiteSpace(SearchString) || (x.Name ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase);
    };

    public async Task ImportExam()
    {
        DialogOptions options = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            Position = DialogPosition.Center,
            DisableBackdropClick = true
        };

        DialogParameters parameters = new() { ["ExamId"] = string.Empty };

        var result = await DialogService.Show<ImportQuestionDialog>("", parameters, options).Result;

        if (result is not null)
        {
            // perform the action here
        }

        await GetExams();
    }

    #endregion
}
