using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.Desktop.Service;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UsersListViewModel : ListViewModelBase<User>, IUsersListViewModel
    {
        private bool _canAddUsers;

        private List<UserGroup> _userGroups;

        private UserGroup _selectedUserGroup;

        private string _login;

        private string _password;

        private readonly IApplicationEvents _appEvents;

        private readonly IUserSession _session;

        private readonly IUsersStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        public UsersListViewModel(
            IUsersStorage storage,
            IApplicationEvents appEvents,
            IUserSession session,
            IMessagesBroker messagesBroker
        )
        {
            _storage = storage;
            _messagesBroker = messagesBroker;

            _appEvents = appEvents;
            _session = session;

            AddUserCommand = ReactiveCommand.CreateFromTask(
                AddUserAsync,
                this.WhenAny(
                    (vm) => vm.Login,
                    (vm) => vm.Password,
                    (login, pass) => {
                        if (string.IsNullOrWhiteSpace(Login)) return false;
                        if (string.IsNullOrWhiteSpace(Password)) return false;

                        return true;
                    }
                )
            );

            RemoveUserCommand = ReactiveCommand.CreateFromTask(
                RemoveUserAsync,
                this.WhenAny(
                    (vm) => vm.SelectedItem,
                    (vm) => SelectedItem != null
                )
            );

            SwitchInputFieldsCommand = ReactiveCommand.Create(
                () => { CanAddUsers = !CanAddUsers; }
            );
        }

        public ReactiveCommand<Unit, Unit> SwitchInputFieldsCommand { get; }

        public ReactiveCommand<Unit, Unit> AddUserCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveUserCommand { get; }

        public UserGroup SelectedUserGroup
        {
            get => _selectedUserGroup;
            set => this.RaiseAndSetIfChanged(ref _selectedUserGroup, value);
        }

        public override User SelectedItem
        {
            get => _selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedItem, value);
                _appEvents.RaiseUserSelected(_selectedItem);
            }
        }

        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public bool CanAddUsers
        {
            get => _canAddUsers;
            set => this.RaiseAndSetIfChanged(ref _canAddUsers, value);
        }

        public List<UserGroup> UserGroups
        {
            get => _userGroups;
            set => this.RaiseAndSetIfChanged(ref _userGroups, value);
        }

        public override void LoadItemsFromList(IEnumerable<User> items)
        {
            ItemsToDisplay.Clear();

            foreach (var item in items)
                ItemsToDisplay.Add(item);

            if (ItemsToDisplay.Count == 0) {
                AreThereItems = false;
                NoItemsMessage = "Пользователи не найдены";
            }
            else AreThereItems = true;
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

        public async void UpdateList()
        {
            try
            {
                await LoadItems();

                if (ItemsToDisplay.Count > 0) {
                    AreThereItems = true;
                    SelectedItem = ItemsToDisplay[0];
                }
                else {
                    AreThereItems = false;
                    NoItemsMessage = "Нет пользователей";
                }
            }
            catch (ArgumentException)
            {
                AreThereItems = false;
                NoItemsMessage = "Ошибка доступа";
            }
            catch
            {
                AreThereItems = false;
                NoItemsMessage = "Что-то пошло не так";
            }
            finally { AreItemsLoaded = true; }
        }

        private async Task LoadItems()
        {
            AreItemsLoaded = false;

            ItemsCache = await _storage.GetUsersAsync();
            ItemsToDisplay.Clear();
            foreach (var user in ItemsCache)
                ItemsToDisplay.Add(user);
        }

        private async Task AddUserAsync()
        {
            try
            {
                await _storage.AddUserAsync(new UserToCreate {
                    Login = Login,
                    Password = Password,
                    GroupID = SelectedUserGroup.ID
                });

                _messagesBroker.NotifyUser($"Пользователь \"{Login}\" добавлен");

                UpdateList();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        private async Task RemoveUserAsync()
        {
            try
            {
                await _storage.RemoveUserAsync(SelectedItem.Login);

                _messagesBroker.NotifyUser($"Пользователь \"{SelectedItem.Login}\" удалён");

                RemoveFromList(SelectedItem);
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
