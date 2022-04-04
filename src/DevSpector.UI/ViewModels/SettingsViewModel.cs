using System.Reactive;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public SettingsViewModel()
        {

        }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public int Mask { get; set; }

        public string NetworkAddress { get; set; }

        public string CurrentAccessToken { get; set; }

        public ReactiveCommand<Unit, Unit> GenerateNewTokenCommand => null;

        public ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }

        public ReactiveCommand<Unit, Unit> GenerateIPRangeCommand { get; }
    }
}
