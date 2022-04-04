using System.Reactive;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ISettingsViewModel
    {
        string CurrentPassword { get; set; }

        string NewPassword { get; set; }

        int Mask { get; set; }

        string NetworkAddress { get; set; }

        ReactiveCommand<Unit, Unit> GenerateNewTokenCommand { get; }

        ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }

        ReactiveCommand<Unit, Unit> GenerateIPRangeCommand { get; }
    }
}
