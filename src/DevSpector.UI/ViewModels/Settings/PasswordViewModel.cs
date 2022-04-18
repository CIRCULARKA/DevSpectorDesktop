using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class PasswordViewModel : ViewModelBase, IPasswordViewModel
    {
        private string _currentPassword;

        private string _newPassword;

        private readonly IUsersStorage _storage;

        private readonly IUserSession _session;

        private readonly IMessagesBroker _messagesBroker;

        public PasswordViewModel(
            IUsersStorage storage,
            IUserSession session,
            IMessagesBroker messagesBroker,
            IUserRights userRights
        ) : base(userRights)
        {
            _storage = storage;

            _session = session;
            _messagesBroker = messagesBroker;

            ChangePasswordCommand = ReactiveCommand.CreateFromTask(
                ChangePasswordAsync,
                this.WhenAny(
                    (vm) => vm.NewPassword,
                    (newPwd) => {
                        if (NewPassword == null) return false;
                        return NewPassword.Length > 5;
                    }
                )
            );
        }

        public ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }

        public string CurrentPassword
        {
            get => _currentPassword;
            set => this.RaiseAndSetIfChanged(ref _currentPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => this.RaiseAndSetIfChanged(ref _newPassword, value);
        }

        public void EraisePasswordInputs()
        {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
        }

        private async Task ChangePasswordAsync()
        {
            try
            {
                await _storage.ChangePasswordAsync(_session.Login, CurrentPassword, NewPassword);

                _messagesBroker.NotifyUser("Ваш пароль успешно обновлён");

                _session.EndSession();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
