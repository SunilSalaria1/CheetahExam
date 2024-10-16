using CheetahExam.WebUI.Client.Services;
using CheetahExam.WebUI.Shared.Common.Models.Accounts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Accounts;

public partial class Registration
{
    #region Fields

    #region Services

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Inject] IAccountClient AccountClient { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion

    MudForm Form = null!;

    RegisterVm Input { get; set; } = new();

    #endregion

    #region Method

    protected override async Task OnInitializedAsync()
    {
        var AuthenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (AuthenticationState.User.Identity?.IsAuthenticated ?? false)
        {
            NavigationManager.NavigateTo("/");
        }
    }

    #endregion

    #region Helper Method

    private async Task Submit()
    {
        await Form.Validate();

        if (!Form.IsValid) return;

        var result = await AccountClient.RegisterAsync(Input);

        string? snackBarMessage;

        Severity severity;

        if (result.Succeeded)
        {
            snackBarMessage = "You are registered successfully";
            severity = MudBlazor.Severity.Success;

            await AuthenticationStateProvider.SetLoginStateAsync(result.Token!);

            NavigationManager.NavigateTo("/");
        }
        else
        {
            snackBarMessage = result.Errors?.First() ?? "Unable to register the user";
            severity = MudBlazor.Severity.Error;
        }

        Snackbar.Add(snackBarMessage, severity: severity);
    }

    #endregion
}
