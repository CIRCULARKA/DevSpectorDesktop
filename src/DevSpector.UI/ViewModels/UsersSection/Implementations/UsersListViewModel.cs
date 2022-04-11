using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.Providers;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Models;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UsersListViewModel : ListViewModelBase<User>, IUsersListViewModel
    {
        private bool _canAddUsers;

        private List<UserGroup> _userGroups;

        private UserGroup _selectedUserGroup;

        private readonly IApplicationEvents _appEvents;

        private readonly IUserSession _session;

        private readonly IUsersStorage _storage;

        public UsersListViewModel(
            IUsersStorage storage,
            IApplicationEvents appEvents,
            IUserSession session
        )
        {
            _storage = storage;

            _appEvents = appEvents;
            _session = session;

            // RemoveUserCommand = ReactiveCommand.Create(
            // );
        }

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

        public bool CanAddUsers
        {
            get => _canAddUsers;
            set => this.RaiseAndSetIfChanged(ref _canAddUsers, value);
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

        private async Task RemoveUserAsync()
        {
            try
            {
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
