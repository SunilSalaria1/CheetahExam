using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Companies;

public partial class DeleteDialogbox
{
    #region Fields

    #region Parameters

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public string DialogType { get; set; } = null!;

    [Parameter]
    public string UniqueId { get; set; } = null!;

    #endregion

    #region Services

    [Inject] ICompanyClient CompanyClient { get; set; } = null!;

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #endregion

    #region Method

    private async Task Submit()
    {
        var result = await CompanyClient.DeleteAsync(UniqueId);

        string snackBarMessage;

        Severity severity;

        if (result.Errors.Any())
        {
            snackBarMessage = string.Join(',', result.Errors);
            severity = Severity.Warning;
        }
        else
        {
            snackBarMessage = $"{DialogType}d";
            severity = Severity.Success;
            MudDialog.Close(DialogResult.Ok(true));
        }

        Snackbar.Add(snackBarMessage, severity);
    }

    void Cancel() => MudDialog.Cancel();

    #endregion
}
