using System.Reactive;
using ReactiveUI;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IAccessKeyViewModel
    {
        string CurrentAccessToken { get; set; }

        string Password { get; set; }

        ReactiveCommand<Unit, Unit> RevokeTokenCommand { get; }

        void DisplayUserAccessKey(User user);
    }
}
