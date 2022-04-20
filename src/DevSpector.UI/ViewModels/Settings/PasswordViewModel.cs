using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Authorization;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class PasswordViewModel : ViewModelBase, IPasswordViewModel
    {
        private string _currentPassword;

        private string _newPassword;

        private readonly IUserSession _session;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IAuthorizationManager _authManager;

        public PasswordViewModel(
            IUserSession session,
            IAuthorizationManager authManager,
            IMessagesBroker messagesBroker,
            IUserRights userRights
        ) : base(userRights)
        {
            _session = session;
            _messagesBroker = messagesBroker;
            _authManager = authManager;

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
                await _authManager.ChangePasswordAsync(_session.Login, CurrentPassword, NewPassword);

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
