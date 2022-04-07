using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Models;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class DevicesListViewModel : ListViewModelBase<Device>, IDevicesListViewModel
    {
        private readonly IApplicationEvents _appEvents;

        private readonly IUserSession _session;

        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        public DevicesListViewModel(
            IApplicationEvents appEvents,
            IUserSession session,
            IDevicesStorage storage,
            IMessagesBroker messagesBroker
        )
        {
            _appEvents = appEvents;
            _session = session;

            _storage = storage;

            _messagesBroker = messagesBroker;

            DeleteDeviceCommand = ReactiveCommand.CreateFromTask(
                DeleteDeviceAsync,
                this.WhenAny(
                    (vm) => vm.SelectedItem,
                    (vm) => SelectedItem != null
                )
            );
        }

        public ReactiveCommand<Unit, Unit> DeleteDeviceCommand { get; }

        public override Device SelectedItem
        {
            get => _selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedItem, value);

                _appEvents.RaiseDeviceSelected(_selectedItem);
            }
        }

        public override void LoadItemsFromList(IEnumerable<Device> devices)
        {
            Items.Clear();

            foreach (var device in devices)
                Items.Add(device);

            if (Items.Count == 0) {
                AreThereItems = false;
                NoItemsMessage = "Устройства не найдены";
            }
            else AreThereItems = true;
        }

        private async Task LoadItems()
        {
            AreItemsLoaded = false;

            ItemsCache = await _storage.GetDevicesAsync();
            Items.Clear();
            foreach (var device in ItemsCache)
                Items.Add(device);
        }

        public override async void InitializeList()
        {
            try
            {
                await LoadItems();

                if (Items.Count > 0) {
                    AreThereItems = true;
                    SelectedItem = Items[0];
                }
                else {
                    AreThereItems = false;
                    NoItemsMessage = "Нет устройств";
                }
            }
            catch (Exception e)
            {
                AreThereItems = false;
                NoItemsMessage = e.Message;
            }
            finally { AreItemsLoaded = true; }
        }

        private async Task DeleteDeviceAsync()
        {
            try
            {
                await _storage.RemoveDeviceAsync(SelectedItem.InventoryNumber);

                _messagesBroker.NotifyUser($"Устройство \"{SelectedItem.InventoryNumber}\" удалено");

                this.Items.Remove(SelectedItem);
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
