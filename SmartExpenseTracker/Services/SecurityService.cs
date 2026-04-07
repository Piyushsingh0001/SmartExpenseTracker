using SmartExpenseTracker.Helpers;
using SmartExpenseTracker.Repositories;

namespace SmartExpenseTracker.Services;

public class SecurityService
{
    private const string PIN_KEY = "pin_hash";
    private const string PIN_ENABLED_KEY = "pin_enabled";
    private readonly AppSettingsRepository _settings;

    public SecurityService(AppSettingsRepository settings) => _settings = settings;

    public bool IsPINEnabled()
    {
        // Use Preferences for fast synchronous check
        return Preferences.Get(PIN_ENABLED_KEY, false);
    }

    public async Task SetPINAsync(string pin)
    {
        var hash = HashHelper.HashPin(pin);
        await _settings.SetAsync(PIN_KEY, hash);
        Preferences.Set(PIN_ENABLED_KEY, true);
    }

    public async Task<bool> ValidatePINAsync(string pin)
    {
        var storedHash = await _settings.GetAsync(PIN_KEY);
        if (storedHash is null) return false;
        return HashHelper.HashPin(pin) == storedHash;
    }

    public async Task DisablePINAsync()
    {
        await _settings.SetAsync(PIN_KEY, string.Empty);
        Preferences.Set(PIN_ENABLED_KEY, false);
    }
}
