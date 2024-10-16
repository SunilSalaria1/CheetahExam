using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace CheetahExam.WebUI.Client.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    private readonly ILocalStorageService _localStorage;


    public CustomAuthenticationStateProvider(HttpClient httpClient,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("token");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = ParseClaimsFromJwt(savedToken);

        var expiration = claims.FirstOrDefault(c => c.Type == "exp")?.Value;

        if (long.TryParse(expiration, out long expirationUnixTimestamp))
        {
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expirationUnixTimestamp).UtcDateTime;

            if (expirationDate > DateTime.UtcNow)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken.Replace("\"", ""));

                var result = await _httpClient.GetAsync("account/name");

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var user = await result.Content.ReadAsStringAsync();

                var identity = !string.IsNullOrEmpty(user) ? new ClaimsIdentity(claims, "jwt", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType) : new ClaimsIdentity();

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            else await SetLogoutStateAsync(null);
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    /// <summary>
    /// We are changing the authentication state of the user that will change the UI for the current/appearent user
    /// To access the endpoints we need to provide access of all the required endpoints to the actual user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userName"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public async Task ChangeCurrentUserAsync(string userId, string userName, string role)
    {
        var savedToken = await _localStorage.GetItemAsync<string>("token");

        var parsedClaims = ParseClaimsFromJwt(savedToken);

        // Remove existing claims
        var identity = new ClaimsIdentity("jwt");
        identity.AddClaims(parsedClaims.Where(c => c.Type != ClaimTypes.Name && c.Type != ClaimsIdentity.DefaultRoleClaimType));

        // Add updated claims
        identity.AddClaim(new Claim(ClaimTypes.Name, userName));
        identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
        identity.AddClaim(new Claim("claims/customclaim/currentUserId", userId, ClaimValueTypes.String));

        var user = new ClaimsPrincipal(identity);

        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    public async Task SetLoginStateAsync(string token)
    {

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

        var user = new ClaimsPrincipal(identity);

        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        await _localStorage.SetItemAsStringAsync("token", token);
    }

    public async Task SetLogoutStateAsync(AuthenticationState? currentAuthState)
    {
        var currentUserId = currentAuthState?.User.Claims.FirstOrDefault(c => c.Type == "claims/customclaim/currentUserId");

        if (!string.IsNullOrWhiteSpace(currentUserId?.Value))
        {
            var state = await GetAuthenticationStateAsync();

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = null;

        await _localStorage.RemoveItemAsync("token");

        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

        var authState = Task.FromResult(new AuthenticationState(anonymousUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public static IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];

        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }

        var jsonBytes = Convert.FromBase64String(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? ""));
    }
}
