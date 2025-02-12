namespace StudyPlannerApplication.App.Services.Security;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _protectedSessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    public CustomAuthenticationStateProvider(ProtectedSessionStorage protectedSessionStorage)
    {
        _protectedSessionStorage = protectedSessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult =
                await _protectedSessionStorage.GetAsync<UserSessionModel>(AuthenticationConstants
                    .UserSessionFromCookie);
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            if (userSession == null)
                return await Task.FromResult(new AuthenticationState(_anonymous));
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, userSession.UserName),
            new Claim(ClaimTypes.Role, userSession.Role)
        }, AuthenticationConstants.CustomAuthFromCookie));
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationState(UserSessionModel? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            await _protectedSessionStorage.SetAsync(AuthenticationConstants.UserSessionFromCookie, userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, userSession.UserName),
            new Claim(ClaimTypes.Role, userSession.Role)
        }, AuthenticationConstants.CustomAuthFromCookie));
        }
        else
        {
            await _protectedSessionStorage.DeleteAsync(AuthenticationConstants.UserSessionFromCookie);
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task<UserSessionModel> GetUserData()
    {
        UserSessionModel model = new();
        try
        {
            var result =
                await _protectedSessionStorage.GetAsync<UserSessionModel>(AuthenticationConstants
                    .UserSessionFromCookie);
            if (result.Success)
            {
                model = result.Value;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return model;
    }

    public class AuthenticationConstants
    {
        public static string UserSessionFromCookie { get; } = "UserSession";
        public static string CustomAuthFromCookie { get; } = "CustomAuth";
    }
}
