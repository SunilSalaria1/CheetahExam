using CheetahExam.WebUI.Client.Services;
using CheetahExam.WebUI.Shared.Common.Models.Accounts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Accounts;

public partial class Login
{
    #region Fields

    #region Services

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] IAccountClient AccountClient { get; set; } = null!;

    [Inject] CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion

    LoginDto Model { get; set; } = new();

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

    /// <summary>
    /// the form need to be validated before login
    /// </summary>
    /// <returns></returns>
    private async Task Submit()
    {
        var result = await AccountClient.LoginAsync(Model);

        var snackBarMessage = string.Empty;

        MudBlazor.Severity severity = new();

        if (result.Succeeded)
        {
            await AuthenticationStateProvider.SetLoginStateAsync(result.Token!);

            NavigationManager.NavigateTo("/");
        }
        else
        {
            snackBarMessage = result.Errors?.First() ?? "Unable to login the user";
            severity = MudBlazor.Severity.Error;
        }

        Snackbar.Add(snackBarMessage, severity: severity);
    }

    #endregion
}
