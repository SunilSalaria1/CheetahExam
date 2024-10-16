using CheetahExam.WebUI.Client.Services;
using CheetahExam.WebUI.Shared.Common.Models.Users;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace CheetahExam.WebUI.Client.Pages.Users;

public partial class Index
{
    #region Fields

    [Inject] IUsersClient UsersClient { get; set; } = null!;

    [Inject] ICompanyClient CompanyClient { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    [Inject] CustomAuthenticationStateProvider CustomAuthenticationStateProvider { get; set; } = null!;

    bool ISLoading { get; set; } = true;

    List<UserDto> Users { get; set; } = new();

    #endregion

    #region Method

    protected async override Task OnInitializedAsync()
    {
        ISLoading = true;

        await GetUsersData();

        ISLoading = false;
    }

    private async Task GetUsersData()
    {
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var currentUserId = authenticationState.User.Claims.FirstOrDefault(c => c.Type == "claims/customclaim/userid")?.Value;

        var company = await CompanyClient.GetByUserIdAsync(currentUserId);

        var roles = new List<string>()
        {
            Constant.UserRoles.SuperAdmin,
            Constant.UserRoles.Admin
        };

        UsersDto users = await UsersClient.GetAllUsersByCompanyIdAndRolesAsync(company.UniqueId, roles);

        Users = users.Users.ToList();

        StateHasChanged();
    }

    private async Task ChangeCurrentUser(UserDto user)
    {
        // handle null exception and multiple roles issue here

        await CustomAuthenticationStateProvider.ChangeCurrentUserAsync(user.UniqueId!, user.UserName!, user.Roles[0]);

        NavigationManager.NavigateTo(user.Roles[0] == Constant.UserRoles.Admin ? "/exams" : "/users");
    }

    #endregion
}
