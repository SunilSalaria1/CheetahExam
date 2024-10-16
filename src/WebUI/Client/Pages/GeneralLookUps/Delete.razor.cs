using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CheetahExam.WebUI.Client.Pages.GeneralLookUps;

public partial class Delete
{
    #region Parameters

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public bool ISDelete { get; set; }

    [Parameter]
    public string UniqueId { get; set; } = null!;

    #endregion

    #region Services

    [Inject]
    private IGeneralLookUpsClient GeneralLookUpsClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Methods

    private async Task Submit()
    {
        string commandsReturnStatus;
        string errors = string.Empty;

        var result = await GeneralLookUpsClient.DeleteAsync(UniqueId);

        if (result.Errors.Any())
        {
            commandsReturnStatus = Constant.CommandsReturnStatus.Exception;
            errors = string.Join(',', result.Errors);
        }
        else
        {
            commandsReturnStatus = Constant.CommandsReturnStatus.Deleted;
        }

        switch (commandsReturnStatus)
        {
            case "Deleted":
                Snackbar.Add("Deleted successfully!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
                break;

            case "Exception":
                Snackbar.Add(errors, Severity.Error);
                break;

            default:
                break;
        }
    }

    void Cancel() => MudDialog.Cancel();

    #endregion
}
