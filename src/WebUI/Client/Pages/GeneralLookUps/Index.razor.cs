using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace CheetahExam.WebUI.Client.Pages.GeneralLookUps;

public partial class Index
{
    #region Fields

    #region Services

    [Inject] IGeneralLookUpsClient _generalLookUpsClient { get; set; } = null!;

    [Inject] IDialogService _dialog { get; set; } = null!;

    #endregion

    bool ISLoading = true;

    bool ISArchived = false;

    int ActiveIndex = 0;

    string? SearchString;

    ICollection<GeneralLookUpDto>? GeneralLookUps { get; set; }

    List<GeneralLookUpDto> GeneralLookUpForActiveIndex { get; set; } = new();

    List<string> GeneralLookUp_Types { get; set; } = new() { Constant.Common.Loading };

    readonly DialogOptions DialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true };

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync() { await GetGeneralLookUps(); }

    #region Helper

    /// <summary>
    /// Get GeneralLookUps Data
    /// </summary>
    /// <returns></returns>

    private async Task GetGeneralLookUps()
    {
        ISLoading = true;

        var generalLookUpsTask = _generalLookUpsClient.GetAllAsync(ISArchived);

        await Task.WhenAll(generalLookUpsTask);

        GeneralLookUps = await generalLookUpsTask;

        GeneralLookUp_Types = GeneralLookUps
                        .Select(t => t.Type)
                        .Distinct()
                        .OrderBy(x => x)
                        .ToList();

        GeneralLookUpForActiveIndex = GeneralLookUps
                    .Where(u => u.Type == GeneralLookUp_Types[ActiveIndex])
                    .ToList();

        ISLoading = false;

        StateHasChanged();
    }

    /// <summary>
    /// formats type name
    /// replace '_' with a white space
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>

    private string ExtractHeader(string type)
    {
        if (string.IsNullOrEmpty(type)) { return Constant.Common.DefaultHeader; }

        // Replace underscores with spaces, and then replace multiple spaces with a single space
        return Regex.Replace(type.Replace('_', ' '), "([a-z])([A-Z])", "$1 $2");
    }

    /// <summary>
    /// Create new generalloopup
    /// </summary>
    /// <returns></returns>

    private async Task CreateGeneralLookUp()
    {
        GeneralLookUpDto generalLookUpDto = new GeneralLookUpDto { Type = GeneralLookUp_Types[ActiveIndex] };

        await HandleGeneralLookUp(generalLookUpDto, true);
    }

    /// <summary>
    /// edit generalloopup
    /// </summary>
    /// <param name="generalLookUpDto"></param>
    /// <returns></returns>

    private async Task EditGeneralLookUp(GeneralLookUpDto generalLookUpDto) { await HandleGeneralLookUp(generalLookUpDto, false); }

    /// <summary>
    /// Open create and edit dialog based on isCreate value
    /// </summary>
    /// <param name="generalLookUp"></param>
    /// <param name="isCreate"></param>
    /// <returns></returns>

    private async Task HandleGeneralLookUp(GeneralLookUpDto generalLookUp, bool isCreate)
    {
        generalLookUp.Type = GeneralLookUp_Types[ActiveIndex];

        var dialog = await _dialog.ShowAsync<Edit>("", new DialogParameters<Edit>
        {
            { x => x.GeneralLookUp, generalLookUp },
            { x => x.ISCreate, isCreate }
        }, DialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled) { await GetGeneralLookUps(); }
    }

    /// <summary>
    /// Delete or archives(change ISArchive to true) generallookup
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>

    private async Task DeleteGeneralLookUp(string uniqueId) { await HandleGeneralLookUpAction(uniqueId, typeof(Delete)); }

    /// <summary>
    /// Retrive or unarchive(change ISArchive to false)
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>

    private async Task RetrieveGeneralLookUp(string uniqueId) { await HandleGeneralLookUpAction(uniqueId, typeof(Retrieve)); }

    /// <summary>
    /// Open dialog for delete or retrive action
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <param name="dialogType"></param>
    /// <returns></returns>

    private async Task HandleGeneralLookUpAction(string uniqueId, Type dialogType)
    {
        var parameters = new DialogParameters<object> { { nameof(Delete.UniqueId), uniqueId } };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.ExtraLarge, // Adjust the size as needed (e.g., ExtraLarge)
            NoHeader = true,
            Position = DialogPosition.Center, // Set the position to Top
        };

        var dialog = await _dialog.ShowAsync(dialogType, "", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled) { await GetGeneralLookUps(); }
    }

    /// <summary>
    /// Archive filter
    /// </summary>
    /// <param name="includeArchived"></param>

    public async void HandleArchivedToggle(bool includeArchived)
    {
        ISArchived = includeArchived;
        await GetGeneralLookUps();
    }

    /// <summary>
    /// Change data of grid on change of generallookup type tab
    /// </summary>
    /// <param name="panelIndex"></param>

    private void OnPanelIndexChanged(int panelIndex)
    {
        if (ActiveIndex != panelIndex)
        {
            ActiveIndex = panelIndex;
            GeneralLookUpForActiveIndex = GeneralLookUps!
                        .Where(u => u.Type == GeneralLookUp_Types[ActiveIndex])
                        .ToList();
        }
    }

    /// <summary>
    /// Search filter
    /// </summary>

    private Func<GeneralLookUpDto, bool> QuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;

        if ((x.Value ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if ((x.Description ?? "").Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    #endregion

    #endregion
}
