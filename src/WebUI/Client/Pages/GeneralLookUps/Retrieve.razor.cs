using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.GeneralLookUps;

public partial class Retrieve
{
    #region Properties

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IGeneralLookUpsClient GeneralLookUpsClient { get; set; } = null!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    #endregion

    #region Fields

    [Parameter]
    public string UniqueId { get; set; } = null!;

    #endregion

    #region Methods

    private async Task Submit()
    {
        string commandsReturnStatus;
        string errors = string.Empty;

        var result = await GeneralLookUpsClient.RetrieveAsync(UniqueId);

        if (result.Errors.Any())
        {
            commandsReturnStatus = Constant.CommandsReturnStatus.Exception;
            errors = string.Join(',', result.Errors);
        }
        else
        {
            commandsReturnStatus = Constant.CommandsReturnStatus.Retrieved;
        }

        switch (commandsReturnStatus)
        {
            case "Retrieved":
                Snackbar.Add("Retrieved successfully!", Severity.Success);
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
