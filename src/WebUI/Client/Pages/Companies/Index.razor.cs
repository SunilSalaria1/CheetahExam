using CheetahExam.WebUI.Shared.Common.Models.Companies;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Companies;

public partial class Index
{
    #region Fields

    #region Services

    [Inject] ICompanyClient CompanyClient { get; set; } = null!;

    [Inject] IDialogService DialogService { get; set; } = null!;

    #endregion

    private bool ISLoading = true;

    /// <summary>
    /// will use this when we will retrive the Deleted or InActive companies
    /// </summary>
    private bool ISActive = true;

    private string? SearchString { get; set; }

    private string? DialogImageURL { get; set; }

    private bool ISImageDialogVisible = false;

    private readonly DialogOptions ImageDialogOptions = new() { CloseButton = true, NoHeader = true, MaxWidth = MaxWidth.Medium };

    private List<CompanyDto> Companies { get; set; } = new();

    #endregion

    #region Methods

    protected async override Task OnInitializedAsync() { await GetCompanies(); }

    #region Helper Method

    private async Task GetCompanies()
    {
        ISLoading = true;

        var companyTask = CompanyClient.GetAllAsync(ISActive);

        await Task.WhenAll(companyTask);

        CompanyCollectionDto companyCollection = await companyTask;

        Companies = companyCollection.Companies;

        ISLoading = false;

        StateHasChanged();
    }

    private async Task HandleActiveToggle(bool isActive)
    {
        ISActive = isActive;
        await GetCompanies();
    }

    private async Task HandleCompanyDeleteAction(string? uniqueId, string dialogType)
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraLarge,
            NoHeader = true,
            Position = DialogPosition.Center,
        };

        var dialog = await DialogService.ShowAsync(typeof(DeleteDialogbox), "", new DialogParameters<DeleteDialogbox>
        {
            { x => x.UniqueId, uniqueId },
            { x => x.DialogType, dialogType }
        }, options);

        var result = await dialog.Result;

        if (!result.Canceled) { await GetCompanies(); }
    }

    /// <summary>
    /// Image Popup
    /// </summary>
    /// <param name="imageUrl"></param>

    private void PopupImage(string imageUrl)
    {
        DialogImageURL = imageUrl;

        ISImageDialogVisible = true;

        StateHasChanged();
    }

    private void ClosePopupImage() => ISImageDialogVisible = false;

    private Func<CompanyDto, bool> QuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;

        if ((x.Name ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ((x.City ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ((x.State ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ((x.PhoneNumber ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ((x.SupportContact ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    #endregion

    #endregion
}
