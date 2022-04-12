using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UserInfoViewModel : ViewModelBase, IUserInfoViewModel
    {
        private string _accessToken;

        private string _login;

        private string _group;

        private string _firstName;

        private string _surname;

        private string _patronymic;

        private UserGroup _selectedUserGroup;

        private List<UserGroup> _userGroups;

        private readonly IUsersStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IUsersListViewModel _usersViewModel;

        public UserInfoViewModel(
            IUsersStorage storage,
            IMessagesBroker messagesBroker,
            IUsersListViewModel usersListViewModel
        )
        {
            _storage = storage;
            _messagesBroker = messagesBroker;
            _usersViewModel = usersListViewModel;

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

                        return Login != selected.Login ||
                            FirstName != selected.FirstName ||
                            Surname != selected.Surname ||
                            Patronymic != selected.Patronymic ||
                            SelectedUserGroup?.Name != selected.Group;
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

        public string Group
        {
            get { return _group == null ? "N/A" : _group; }
            set => this.RaiseAndSetIfChanged(ref _group, value);
        }

        public string Login
        {
            get { return _login == null ? "N/A" : _login; }
            set => this.RaiseAndSetIfChanged(ref _login, value);
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

        public void UpdateUserInfo(User target)
        {
            AccessToken = target?.AccessToken;
            Group = target?.Group;
            Login = target?.Login;
            FirstName = target?.FirstName;
            Surname = target?.Surname;
            Patronymic = target?.Patronymic;
        }

        public async Task LoadUserGroupsAsync()
        {
            try
            {
                UserGroups = await _storage.GetUserGroupsAsync();
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

                await _storage.UpdateUserAsync(
                    selectedUser.Login,
                    new UserToCreate {
                        FirstName = FirstName,
                        Surname = Surname,
                        Patronymic = Patronymic,
                        Login = Login,
                        GroupID = SelectedUserGroup.ID
                    }
                );
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
