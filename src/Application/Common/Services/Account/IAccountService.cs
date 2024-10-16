namespace CheetahExam.Application.Common.Services.Account;

public interface IAccountService
{
    string GenerateToken(string userId, string username, List<string> roles);

    string CurrentUser();
}
