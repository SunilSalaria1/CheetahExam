using Microsoft.AspNetCore.Components;

namespace CheetahExam.WebUI.Client.Pages;

public partial class CustomDialog
{
    [Parameter] public string Message { get; set; }
    [Parameter] public string Description { get; set; }
    [Parameter] public Action<bool> OnConfirmation { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    private void Confirm(bool result)
    {
        OnConfirmation?.Invoke(result);
    }

    private string CssClass => "custom-dialog";

    private Dictionary<string, object> Attributes => new() { { "class", CssClass } };
}
