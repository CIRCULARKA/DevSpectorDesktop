using System.Reactive;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IPasswordViewModel
    {
        string CurrentPassword { get; set; }

        string NewPassword { get; set; }

        ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }

        void EraisePasswordInputs();
    }
}
