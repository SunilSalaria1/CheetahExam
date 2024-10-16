using CheetahExam.WebUI.Shared.Common.Models.Users;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CheetahExam.WebUI.Client.Pages.Companies;

public partial class EditUser
{
    #region Fields

    [Parameter] public string? UserId { get; set; }

    [Parameter] public required string CompanyId { get; set; }

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    [Inject] IUsersClient UsersClient { get; set; } = null!;

    [Inject] IRolesClient RolesClient { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    string? UserName;

    MudForm Form = null!;

    UserDto User { get; set; } = new();

    List<RoleDto> Roles { get; set; } = new();

    IEnumerable<string> Options { get; set; } = new List<string>() { };

    #endregion

    #region Method

    protected async override Task OnInitializedAsync()
    {
        await GetCompanyRoles();

        if (!string.IsNullOrWhiteSpace(UserId))
        {
            User = await UsersClient.GetUserAsync(UserId);

            UserName = $"{User.FirstName} {User.LastName}";

            Options = User.Roles;
        }
    }

    /// <summary>
    /// Currently we are keeping less roles. 
    /// Roles will be here based on company
    /// </summary>
    /// <returns></returns>
    private async Task GetCompanyRoles()
    {
        RolesDto roles = await RolesClient.GetAllRolesByCompanyIdAsync(CompanyId);

        Roles = new() { roles.Roles.FirstOrDefault(r => r.Name == Constant.UserRoles.SuperAdmin) ?? new() };

        StateHasChanged();
    }

    private async Task Submit()
    {
        string snackBarMessage;

        Severity severity;

        await Form.Validate();

        if (Form.IsValid)
        {
            User.Roles = Options.ToList();

            if (string.IsNullOrWhiteSpace(UserId))
            {
                var result = await UsersClient.CreateUserAsync(CompanyId, User);

                if (result.Errors.Any())
                {
                    snackBarMessage = $"{User.FirstName} {User.LastName} not created!, {string.Join(',', result.Errors)}";
                    severity = Severity.Warning;
                }
                else
                {
                    snackBarMessage = $"{User.FirstName} {User.LastName} created successfully!";
                    severity = Severity.Success;
                    Back();
                }
            }
            else
            {
                var result = await UsersClient.UpdateUserAsync(UserId, CompanyId, User);
                if (result.Errors.Any())
                {
                    snackBarMessage = $"{User.FirstName} {User.LastName} not updated!, {string.Join(',', result.Errors)}";
                    severity = Severity.Warning;
                }
                else
                {
                    snackBarMessage = $"{User.FirstName} {User.LastName} updated successfully!";
                    severity = Severity.Success;
                    Back();
                }
            }
            ShowSnackbar(snackBarMessage, severity);
        }
    }

    private static string GetMultiSelectionText(List<string> selectedValues) => $"{selectedValues.Count} role{(selectedValues.Count > 1 ? "s have" : " has")} been selected";

    private void ShowSnackbar(string message, Severity severity) => Snackbar.Add(message, severity: severity);

    private void Back() => NavigationManager.NavigateTo($"/companies/{CompanyId}");

    #endregion
}
