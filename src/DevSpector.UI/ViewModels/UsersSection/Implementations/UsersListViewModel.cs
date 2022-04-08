using System;
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
        private readonly IApplicationEvents _appEvents;

        private readonly IUsersProvider _usersProvider;

        private readonly IUserSession _session;

        public UsersListViewModel(
            IUsersProvider usersProvider,
            IApplicationEvents appEvents,
            IUserSession session
        )
        {
            _appEvents = appEvents;
            _session = session;
            _usersProvider = usersProvider;
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

        public async void InitializeList()
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

            ItemsCache = await _usersProvider.GetUsersAsync();
            ItemsToDisplay.Clear();
            foreach (var user in ItemsCache)
                ItemsToDisplay.Add(user);
        }

    }
}
