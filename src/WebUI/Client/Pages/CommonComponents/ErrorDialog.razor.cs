using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.CommonComponents;

public partial class ErrorDialog
{
    #region Fields

    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public List<string> Errors { get; set; } = new();

    [Comment("Error - is default Title of this Dialog box")]
    [Parameter] public string Title { get; set; } = "Error";

    #endregion

    #region Methods

    void Cancel() => MudDialog.Close(DialogResult.Ok(true));

    #endregion
}
