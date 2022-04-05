using System;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Editors;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Models;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class DevicesListViewModel : ListViewModelBase<Device>, IDevicesListViewModel
    {
        private readonly IApplicationEvents _appEvents;

        private readonly IDevicesProvider _devicesProvider;

        private readonly IUserSession _session;

        private readonly IDevicesEditor _editor;

        private readonly IMessagesBroker _messagesBroker;

        public DevicesListViewModel(
            IDevicesProvider devicesProvider,
            IApplicationEvents appEvents,
            IUserSession session,
            IDevicesEditor editor,
            IMessagesBroker messagesBroker
        )
        {
            _appEvents = appEvents;
            _session = session;
            _devicesProvider = devicesProvider;

            _editor = editor;

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

        protected override async Task LoadItems()
        {
            AreItemsLoaded = false;

            ItemsCache = await _devicesProvider.GetDevicesAsync();
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

        private async Task DeleteDeviceAsync()
        {
            try
            {
                await _editor.DeleteDeviceAsync(SelectedItem.InventoryNumber);

                _messagesBroker.NotifyUser($"Устройство \"{SelectedItem.InventoryNumber}\" удалено");

                this.Items.Remove(SelectedItem);
            }
            catch (HttpRequestException)
            {
                _messagesBroker.NotifyUser("Не удалось связаться с сервером");
            }
            catch
            {
                _messagesBroker.NotifyUser( "Что-то пошло не так при удалении устройства");
            }
        }
    }
}
