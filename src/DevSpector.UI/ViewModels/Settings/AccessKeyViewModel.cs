using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Authorization;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class AccessKeyViewModel : ViewModelBase, IAccessKeyViewModel
    {
        private string _currentAccessToken;

        private string _password;

        private readonly IUserSession _session;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IAuthorizationManager _authManager;

        public AccessKeyViewModel(
            IUserSession session,
            IAuthorizationManager authManager,
            IMessagesBroker messagesBroker,
            IUserRights userRights
        ) : base(userRights)
        {
            _session = session;
            _messagesBroker = messagesBroker;
            _authManager = authManager;

            RevokeTokenCommand = ReactiveCommand.CreateFromTask(
                RevokeAccessTokenAsync,
                this.WhenAny(
                    vm => vm.Password,
                    (pwd) => !string.IsNullOrWhiteSpace(Password)
                )
            );
        }

        public ReactiveCommand<Unit, Unit> RevokeTokenCommand { get; }

        public string CurrentAccessToken
        {
            get => _currentAccessToken;
            set => this.RaiseAndSetIfChanged(ref _currentAccessToken, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public void DisplayUserAccessKey(User user)
        {
            CurrentAccessToken = user?.AccessToken;
        }

        public void EraisePasswordInput()
        {
            Password = string.Empty;
        }

        private async Task RevokeAccessTokenAsync()
        {
            try
            {
                string newToken = await _authManager.RevokeKeyAsync(_session.Login, Password);

                CurrentAccessToken = newToken;

                _messagesBroker.NotifyUser("Ваш ключ доступа успешно обновлён");

                _session.EndSession();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
