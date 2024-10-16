using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;
using CheetahExam.WebUI.Shared.Constants;
using CheetahExam.WebUI.Shared.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Exam;

public partial class Edit : ComponentBase
{
    #region Fields

    #region Services

    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IExamsClient ExamsClient { get; set; } = null!;
    [Inject] IFilesClient FilesClient { get; set; } = null!;
    [Inject] IGeneralLookUpsClient GeneralLookUpsClient { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    [Parameter] public string? ExamId { get; set; }

    private ExamDto model = new ExamDto();
    MudForm form = new();

    private FileDetailDto? fileDetail;
    private string? SelectedImage;
    private string? ImageUrl;

    private List<KeyValue> DisciplineItems { get; set; } = new List<KeyValue>();
    private string? SelectedDiscipline { get; set; }

    private List<KeyValue> CategoryItems { get; set; } = new List<KeyValue>();
    private string? SelectedCategory { get; set; }

    private List<KeyValue> ExamResultItems { get; set; } = new List<KeyValue>();
    private string? SelectedResultType { get; set; }
    private string? ResultTypeValue { get; set; }

    private List<KeyValue> FontItems { get; set; } = new List<KeyValue>();
    private string? SelectedFont { get; set; }

    #endregion

    #region Methods

    protected async override Task OnInitializedAsync()
    {
        await BindDropDownList();

        if (!string.IsNullOrEmpty(ExamId))
        {
            model = await ExamsClient.GetByIdAsync(ExamId);
            SelectedCategory = model?.Category_GeneralLookUpID?.ToString();
            SelectedDiscipline = model?.Discipline_GeneralLookUpID?.ToString();
            SelectedResultType = model?.Result_GeneralLookUpID?.ToString();
            SelectedFont = model?.FontStyle;

            ResultTypeValue = ExamResultItems
                .FirstOrDefault(p => p.Key == Convert.ToInt32(SelectedResultType))?.Value;

            if (model?.Media?.Url != null)
            {
                ImageUrl = model.Media.Url;
                SelectedImage = $"{ImageUrl.Replace(Constant.FileDirectory.ExamImage, "").Substring(0, 17)}...";
            }
        }
    }

    private async Task BindDropDownList()
    {
        CategoryItems = (await GeneralLookUpsClient.GetByTypeAsync(Constant.LookUpTypes.Category_Type)).ToList();
        DisciplineItems = (await GeneralLookUpsClient.GetByTypeAsync(Constant.LookUpTypes.Discipline_Type)).ToList();
        ExamResultItems = (await GeneralLookUpsClient.GetByTypeAsync(Constant.LookUpTypes.Result_Type)).ToList();
        FontItems = (await ExamsClient.GetAllFontsAsync()).Fonts.Select(font => new KeyValue() { Key = font.Id, Value = font.Name }).ToList();
    }

    private void HandleCategoryValueChanged(string selectedCategory)
    {
        SelectedCategory = selectedCategory;
        model.Category_GeneralLookUpID = SelectedCategory == null ? null : Convert.ToInt32(SelectedCategory);
    }

    private void HandleDisciplineValueChanged(string selectedDiscipline)
    {
        SelectedDiscipline = selectedDiscipline;
        model.Discipline_GeneralLookUpID = SelectedDiscipline == null ? null : Convert.ToInt32(SelectedDiscipline);
    }

    private void HandleResultTypeChanged(string value)
    {
        SelectedResultType = value;
        model.PassingScore = null;
        model.Result_GeneralLookUpID = SelectedResultType == null ? null : Convert.ToInt32(SelectedResultType);

        ResultTypeValue = ExamResultItems
            .FirstOrDefault(p => p.Key == Convert.ToInt32(SelectedResultType))?.Value;

        model.ExamResultOptions = ResultTypeValue switch
        {
            Constant.ResultType.LetterGrading => CommonHelper.GenerateLetterGradingOptions(),
            Constant.ResultType.GoodExcellent => CommonHelper.GenerateGoodExcellentOptions(),
            _ => new List<ExamResultOptionDto>()
        };
    }

    private void HandleFontFamilyChanged(string selectedFont)
    {
        SelectedFont = selectedFont;
        model.FontStyle = selectedFont;
    }

    private async Task Submit()
    {
        await form.Validate();

        if (form.IsValid)
        {
            if (fileDetail != null && IsImageFile(fileDetail.MetaData.FileType))
            {
                model.Media = await FilesClient.UploadAsync(fileDetail);
            }

            if (string.IsNullOrWhiteSpace(ExamId))
            {
                await HandleCreateAsync();
            }
            else
            {
                await HandleUpdateAsync();
            }
        }
    }

    private bool IsImageFile(string fileType) =>
        fileType is Constant.FileTypes.JPEG or Constant.FileTypes.PNG or Constant.FileTypes.GIF or Constant.FileTypes.JPG;

    private async Task HandleCreateAsync()
    {
        var examId = await ExamsClient.CreateAsync(model);
        ShowConfirmationDialog(examId);
    }

    private async Task HandleUpdateAsync()
    {
        if (model.Media != null && ShouldDeletePreviousImage())
        {
            await FilesClient.DeleteAsync(ImageUrl);
        }

        string commandReturnStatus = await ExamsClient.UpdateAsync(ExamId, model);
        HandleUpdateResult(commandReturnStatus);
    }

    private void HandleUpdateResult(string commandReturnStatus)
    {
        string snackBarMessage = string.Empty;
        Severity severity = new();

        switch (commandReturnStatus)
        {
            case "Updated":
                ShowConfirmationDialog(ExamId);
                break;

            case "NotFound":
                snackBarMessage = "not found!";
                severity = Severity.Warning;
                break;

            case "AlreadyExist":
                snackBarMessage = "already exist!";
                severity = Severity.Warning;
                break;

            default:
                break;
        }

        if (!string.IsNullOrEmpty(snackBarMessage))
        {
            ShowSnackbar($"{model.Name} {snackBarMessage}", severity);
        }
    }

    private async void ShowConfirmationDialog(string examId)
    {
        var result = await DialogService.ShowAsync<CustomDialog>("Confirmation", new DialogParameters
        {
            { "Message", "Do you want to create questions for this exam?" },
            { "Description", "If you'd like to create questions based on this exam, click 'Accept'. If you prefer to do this later, click 'Decline'." },
            { "OnConfirmation", new Action<bool>(confirmed => HandleConfirmation(confirmed, examId)) }
        });
    }

    private void HandleConfirmation(bool result, string examId)
    {
        if (result)
        {
            NavigateToQuestionsPage(examId);
            ShowSnackbar($"{model.Name} created!", Severity.Success);
        }
        else
        {
            NavigationManager.NavigateTo($"/exams");
            ShowSnackbar($"{model.Name} created!", Severity.Success);
        }
    }

    private bool ShouldDeletePreviousImage() =>
        model.Media?.Url != ImageUrl && ImageUrl != null;

    private void NavigateToQuestionsPage(string examId) =>
        NavigationManager.NavigateTo($"/exams/questions/{examId}");

    private void ShowSnackbar(string message, Severity severity) =>
        Snackbar.Add(message, severity: severity);

    private void Cancel() => NavigationManager.NavigateTo($"/exams");

    private async Task UploadImage(InputFileChangeEventArgs e)
    {
        fileDetail = new();

        var buffer = new byte[e.File.Size];

        await e.File.OpenReadStream(maxAllowedSize: 10000 * 10000).ReadAsync(buffer);

        fileDetail.MetaData.FileName = e.File.Name;
        fileDetail.MetaData.FileSize = e.File.Size;
        fileDetail.MetaData.FileType = e.File.ContentType;
        fileDetail.MetaData.FileBytes = buffer;

        fileDetail.Path = Constant.FileDirectory.ExamImage;

        SelectedImage = (e.File.Name.Length > 20) ? e.File.Name.Substring(0, 17) + "..." : e.File.Name;
    }

    private void ClearImage()
    {
        fileDetail = null;
        SelectedImage = null;
        if (model.Media != null)
        {
            model.Media.Url = null;
        }
    }

    #endregion
}
