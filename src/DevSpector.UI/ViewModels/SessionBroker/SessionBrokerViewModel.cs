using System.Reactive;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SessionBrokerViewModel : ViewModelBase, ISessionBrokerViewModel
    {
        private string _loggedUserLogin;

        private string _loggedUserGroup;

        private readonly IUserSession _session;

        public SessionBrokerViewModel(
            IUserSession session
        )
        {
            _session = session;

            LogoutCommand = ReactiveCommand.Create(
                () => _session.EndSession()
            );
        }

        public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

        public string LoggedUserLogin
        {
            get => _loggedUserLogin;
            set => this.RaiseAndSetIfChanged(ref _loggedUserLogin, value);
        }

        public string LoggedUserGroup
        {
            get => _loggedUserGroup;
            set => this.RaiseAndSetIfChanged(ref _loggedUserGroup, value);
        }

        public void UpdateLoggedUserInfo(User user)
        {
            LoggedUserLogin = user.Login;
            LoggedUserGroup = user.Group;
        }
    }
}
