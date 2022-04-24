using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using DevSpector.Desktop.Service;
using DevSpector.Desktop.UI.Validators;
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

        private readonly ITextValidator _textValidator;

        public PasswordViewModel(
            IUserSession session,
            IAuthorizationManager authManager,
            IMessagesBroker messagesBroker,
            IUserRights userRights,
            ITextValidator textValidator
        ) : base(userRights)
        {
            _session = session;
            _messagesBroker = messagesBroker;
            _authManager = authManager;
            _textValidator = textValidator;

            ChangePasswordCommand = ReactiveCommand.CreateFromTask(
                ChangePasswordAsync,
                this.WhenAny(
                    (vm) => vm.NewPassword,
                    (newPwd) => {
                        if (NewPassword == null) return false;
                        if (!_textValidator.IsValid(NewPassword)) return false;
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
            set
            {
                this.RaiseAndSetIfChanged(ref _newPassword, value);
                _textValidator.Validate(value);
            }
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
