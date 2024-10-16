using CheetahExam.Application.Common.Services.Account;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CheetahExam.WebUI.Server.Services;

public class AccountService : IAccountService
{
    #region Fields

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    #endregion

    #region Ctor

    public AccountService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    #endregion

    #region Methods

    public string GenerateToken(string userId, string userName, List<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (string.IsNullOrEmpty(_configuration["JWT:Secret"])) return "";

        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);

        var claims = new List<Claim> { new(ClaimTypes.Name, userName), new("claims/customclaim/userid", userId, "string") };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:ExpiryTime")),
            signingCredentials: signingCredentials);

        var encryptedToken = tokenHandler.WriteToken(token);

        return encryptedToken;
    }

    public string CurrentUser()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue("claims/customclaim/userid") ?? "";
    }

    #endregion
}
