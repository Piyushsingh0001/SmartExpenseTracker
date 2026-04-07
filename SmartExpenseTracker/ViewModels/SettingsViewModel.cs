using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpenseTracker.Services;
using static System.Net.WebRequestMethods;

namespace SmartExpenseTracker.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly SecurityService _security;
    private readonly BackupService _backup;

    [ObservableProperty] private bool _isPinEnabled;
    [ObservableProperty] private string _pinInput = string.Empty;

    public SettingsViewModel(SecurityService security, BackupService backup)
    {
        _security = security;
        _backup = backup;
        IsPinEnabled = _security.IsPINEnabled();
        Title = "Settings";
    }

    [RelayCommand]
    private async Task SetPinAsync()
    {
        if (PinInput.Length != 4 || !PinInput.All(char.IsDigit))
        {
            await Shell.Current.DisplayAlert("Invalid PIN", "Enter exactly 4 digits.", "OK");
            return;
        }
        await _security.SetPINAsync(PinInput);
        IsPinEnabled = true;
        PinInput = string.Empty;
        await Shell.Current.DisplayAlert("Success", "PIN set successfully.", "OK");
    }

    [RelayCommand]
    private async Task DisablePinAsync()
    {
        await _security.DisablePINAsync();
        IsPinEnabled = false;
        await Shell.Current.DisplayAlert("Disabled", "PIN lock disabled.", "OK");
    }

    [RelayCommand]
    private async Task BackupAsync()
    {
        try
        {
            var path = await _backup.BackupAsync();
            await Shell.Current.DisplayAlert("Backup", $"Saved to:\n{path}", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task ExportExcelAsync()
    {
        try
        {
            var path = await _backup.ExportToExcelAsync();
            await Shell.Current.DisplayAlert("Exported", $"Excel saved to:\n{path}", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task RestoreAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select backup .db3 file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/octet-stream" } },
                    { DevicePlatform.iOS, new[] { "public.database" } }
                })
            });
            if (result is null) return;
            await _backup.RestoreAsync(result.FullPath);
            await Shell.Current.DisplayAlert("Restored", "Backup restored. Restart the app.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
