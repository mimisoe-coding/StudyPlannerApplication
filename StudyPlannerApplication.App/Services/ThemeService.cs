using Microsoft.JSInterop;

namespace StudyPlannerApplication.App.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = "light";
    private string? _customPrimaryColor;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public string CurrentTheme => _currentTheme;
    public string? CustomPrimaryColor => _customPrimaryColor;

    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

    public async Task InitializeAsync()
    {
        _currentTheme = await _jsRuntime.InvokeAsync<string>("themeManager.getTheme");
        _customPrimaryColor = await _jsRuntime.InvokeAsync<string?>("themeManager.getPrimaryColor");
        NotifyStateChanged();
    }

    public async Task ToggleThemeAsync()
    {
        _currentTheme = _currentTheme == "light" ? "dark" : "light";
        await _jsRuntime.InvokeVoidAsync("themeManager.setTheme", _currentTheme);
        NotifyStateChanged();
    }

    public async Task SetPrimaryColorAsync(string? hex)
    {
        _customPrimaryColor = hex;
        await _jsRuntime.InvokeVoidAsync("themeManager.setPrimaryColor", hex);
        NotifyStateChanged();
    }
}
