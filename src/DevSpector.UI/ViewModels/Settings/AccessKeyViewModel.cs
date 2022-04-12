using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class AccessKeyViewModel : ViewModelBase, IAccessKeyViewModel
    {
        private string _currentAccessToken;

        private string _password;

        private readonly IUsersStorage _storage;

        private readonly IUserSession _session;

        private readonly IMessagesBroker _messagesBroker;

        public AccessKeyViewModel(
            IUsersStorage storage,
            IUserSession session,
            IMessagesBroker messagesBroker
        )
        {
            _storage = storage;

            _session = session;
            _messagesBroker = messagesBroker;

            RevokeTokenCommand = ReactiveCommand.CreateFromTask(
                RevokeAccessTokenAsync
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

        private async Task RevokeAccessTokenAsync()
        {
            try
            {
                string newToken = await _storage.RevokeAccessKeyAsync(_session.Login, Password);

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
