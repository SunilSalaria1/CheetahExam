using CheetahExam.WebUI.Shared.Common.Models.Companies;
using CheetahExam.WebUI.Shared.Common.Models.Users;
using CheetahExam.WebUI.Shared.Constants;
using Microsoft.AspNetCore.Components;

namespace CheetahExam.WebUI.Client.Pages.Companies;

public partial class View
{
    #region Fields

    [Parameter] public string? UniqueId { get; set; }

    [Inject] ICompanyClient CompanyClient { get; set; } = null!;

    [Inject] IUsersClient UsersClient { get; set; } = null!;

    bool ISLoading { get; set; } = true;

    List<UserDto> Users { get; set; } = new();

    CompanyDto Company { get; set; } = new();

    #endregion

    #region Method

    protected async override Task OnInitializedAsync()
    {
        ISLoading = true;

        Company = await CompanyClient.GetByIdAsync(UniqueId);

        await GetUsersData();

        ISLoading = false;
    }

    private async Task GetUsersData()
    {
        var roles = new List<string>()
        {
            Constant.UserRoles.SuperAdmin
        };

        UsersDto users = await UsersClient.GetAllUsersByCompanyIdAndRolesAsync(UniqueId, roles);

        Users = users.Users.ToList();

        StateHasChanged();
    }

    #endregion
}
