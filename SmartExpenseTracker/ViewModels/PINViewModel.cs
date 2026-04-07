using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpenseTracker.Services;
using System.Net.NetworkInformation;
using static System.Net.WebRequestMethods;

namespace SmartExpenseTracker.ViewModels;

public partial class PINViewModel : BaseViewModel
{
    private readonly SecurityService _security;

    [ObservableProperty] private string _pin = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public PINViewModel(SecurityService security)
    {
        _security = security;
        Title = "Enter PIN";
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (Pin.Length != 4)
        {
            ErrorMessage = "PIN must be 4 digits.";
            return;
        }
        bool valid = await _security.ValidatePINAsync(Pin);
        if (valid)
        {
            // Navigate back to dashboard
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            ErrorMessage = "Incorrect PIN. Try again.";
            Pin = string.Empty;
        }
    }
}
