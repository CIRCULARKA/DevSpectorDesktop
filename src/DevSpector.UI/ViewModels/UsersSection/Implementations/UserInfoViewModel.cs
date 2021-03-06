using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Editors;
using DevSpector.Desktop.Service;
using DevSpector.Desktop.UI.Validators;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UserInfoViewModel : ViewModelBase, IUserInfoViewModel
    {
        private string _accessToken;

        private string _login;

        private string _firstName;

        private string _surname;

        private string _patronymic;

        private UserGroup _selectedUserGroup;

        private List<UserGroup> _userGroups;

        private bool _canEditUser;

        private readonly ApplicationEvents _appEvents;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IUsersListViewModel _usersViewModel;

        private readonly IUsersEditor _usersEditor;

        private readonly IUsersProvider _usersProvider;

        private readonly ITextValidator _textValidator;

        public UserInfoViewModel(
            IMessagesBroker messagesBroker,
            IUsersListViewModel usersListViewModel,
            IUsersEditor usersEditor,
            IUsersProvider usersProvider,
            IUserRights userRights,
            ITextValidator textValidator,
            ApplicationEvents appEvents
        ) : base(userRights)
        {
            _messagesBroker = messagesBroker;
            _usersViewModel = usersListViewModel;
            _usersEditor = usersEditor;
            _usersProvider = usersProvider;
            _appEvents = appEvents;
            _textValidator = textValidator;

            ApplyChangesCommand = ReactiveCommand.CreateFromTask(
                UpdateUserAsync,
                this.WhenAny(
                    (vm) => vm.Login,
                    (vm) => vm.FirstName,
                    (vm) => vm.Surname,
                    (vm) => vm.Patronymic,
                    (vm) => vm.SelectedUserGroup,
                    (p1, p2, p3, p4, p5) => {
                        User selected = _usersViewModel.SelectedItem;
                        if (selected == null) return false;

                        if (!_textValidator.IsValid(Login)) return false;
                        if (Login != selected.Login) return true;
                        if (FirstName != selected.FirstName) return true;
                        if (Surname != selected.Surname) return true;
                        if (Patronymic != selected.Patronymic) return true;
                        if (SelectedUserGroup == null) return false;
                        if (SelectedUserGroup.Name != selected.Group) return true;
                        return false;
                    }
                )
            );
        }

        public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

        public UserGroup SelectedUserGroup
        {
            get => _selectedUserGroup;
            set => this.RaiseAndSetIfChanged(ref _selectedUserGroup, value);
        }

        public List<UserGroup> UserGroups
        {
            get => _userGroups;
            set => this.RaiseAndSetIfChanged(ref _userGroups, value);
        }

        public string AccessToken
        {
            get { return _accessToken == null ? "" : _accessToken; }
            set => this.RaiseAndSetIfChanged(ref _accessToken, value);
        }

        public string Login
        {
            get => _login;
            set
            {
                this.RaiseAndSetIfChanged(ref _login, value);
                _textValidator.Validate(value);
            }
        }

        public string FirstName
        {
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        public string Surname
        {
            get => _surname;
            set => this.RaiseAndSetIfChanged(ref _surname, value);
        }

        public string Patronymic
        {
            get => _patronymic;
            set => this.RaiseAndSetIfChanged(ref _patronymic, value);
        }

        public bool CanEditUser
        {
            get => _canEditUser;
            set => this.RaiseAndSetIfChanged(ref _canEditUser, value);
        }

        public void UpdateUserInfo(User target)
        {
            AccessToken = target?.AccessToken;
            Login = target?.Login;
            FirstName = target?.FirstName;
            Surname = target?.Surname;
            Patronymic = target?.Patronymic;
            SelectedUserGroup = UserGroups?.FirstOrDefault(ug => ug.Name == target?.Group);
        }

        public async Task LoadUserGroupsAsync()
        {
            try
            {
                UserGroups = await _usersProvider.GetUserGroupsAsync();
                SelectedUserGroup = UserGroups.FirstOrDefault();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        private async Task UpdateUserAsync()
        {
            try
            {
                User selectedUser = _usersViewModel.SelectedItem;

                string newFirstName = FirstName == selectedUser.FirstName ?
                    null : FirstName;

                string newSurname = Surname == selectedUser.Surname ?
                    null : Surname;

                string newPatronymic = Patronymic == selectedUser.Patronymic ?
                    null : Patronymic;

                string newLogin = Login == selectedUser.Login ?
                    null : Login;

                string newGroupID = SelectedUserGroup?.Name == selectedUser.Group ?
                    null : SelectedUserGroup.ID;

                await _usersEditor.UpdateUserAsync(
                    selectedUser.Login,
                    new UserToCreate {
                        FirstName = newFirstName,
                        Surname = newSurname,
                        Patronymic = newPatronymic,
                        Login = newLogin,
                        GroupID = newGroupID
                    }
                );

                _appEvents.RaiseUserUpdated(selectedUser.ID);

                _messagesBroker.NotifyUser($"???????????????????????? \"{selectedUser.Login}\" ????????????????");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        public void UpdateInputsAccessibility() =>
            CanEditUser = _usersViewModel.SelectedItem != null;
    }
}
