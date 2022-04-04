using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ISettingsViewModel
    {
        string CurrentPassword { get; set; }

        string NewPassword { get; set; }

        int Mask { get; set; }

        string NetworkAddress { get; set; }

        ReactiveCommand GenerateNewTokenCommand { get; }

        ReactiveCommand ChangePasswordCommand { get; }

        ReactiveCommand GenerateIPRangeCommand { get; }
    }
}
