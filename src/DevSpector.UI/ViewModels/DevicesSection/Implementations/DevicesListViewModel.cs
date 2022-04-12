using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.Desktop.Service;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using ReactiveUI;
using Avalonia.Data;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class DevicesListViewModel : ListViewModelBase<Device>, IDevicesListViewModel
    {
        private readonly IApplicationEvents _appEvents;

        private readonly IUserSession _session;

        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        private string _inventoryNumber;

        private string _modelName;

        private bool _canAddDevice;

        private DeviceType _selectedDeviceType;

        private List<DeviceType> _deviceTypes;

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

            SwitchInputFieldsCommand = ReactiveCommand.CreateFromTask(
                async () => {
                    CanAddDevice = !CanAddDevice;

                    if (!CanAddDevice) return;

                    await LoadDeviceTypesAsync();
                    SelectedDeviceType = DeviceTypes.FirstOrDefault();
                }
            );

            AddDeviceCommand = ReactiveCommand.CreateFromTask(
                AddDeviceAsync
            );

            DeleteDeviceCommand = ReactiveCommand.CreateFromTask(
                DeleteDeviceAsync,
                this.WhenAny(
                    (vm) => vm.SelectedItem,
                    (vm) => SelectedItem != null
                )
            );
        }

        public ReactiveCommand<Unit, Unit> SwitchInputFieldsCommand { get; }

        public ReactiveCommand<Unit, Unit> AddDeviceCommand { get; }

        public ReactiveCommand<Unit, Unit> DeleteDeviceCommand { get; }

        public bool CanAddDevice
        {
            get => _canAddDevice;
            set => this.RaiseAndSetIfChanged(ref _canAddDevice, value);
        }

        public List<DeviceType> DeviceTypes
        {
            get => _deviceTypes;
            set => this.RaiseAndSetIfChanged(ref _deviceTypes, value);
        }

        public string InventoryNumber
        {
            get => _inventoryNumber;
            set
            {
                this.RaiseAndSetIfChanged(ref _inventoryNumber, value);
            }
        }

        public string ModelName
        {
            get => _modelName;
            set => this.RaiseAndSetIfChanged(ref _modelName, value);
        }

        public DeviceType SelectedDeviceType
        {
            get => _selectedDeviceType;
            set => this.RaiseAndSetIfChanged(ref _selectedDeviceType, value);
        }

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
            ItemsToDisplay.Clear();

            foreach (var device in devices)
                ItemsToDisplay.Add(device);

            if (ItemsToDisplay.Count == 0) {
                AreThereItems = false;
                NoItemsMessage = "Устройства не найдены";
            }
            else AreThereItems = true;
        }

        private async Task LoadItems()
        {
            AreItemsLoaded = false;

            ItemsCache = await _storage.GetDevicesAsync();
            ItemsToDisplay.Clear();
            foreach (var device in ItemsCache)
                ItemsToDisplay.Add(device);
        }

        public async void UpdateList()
        {
            try
            {
                await LoadItems();

                if (ItemsToDisplay.Count > 0)
                {
                    AreThereItems = true;
                    SelectedItem = ItemsToDisplay[0];
                }
                else
                {
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

        public void AddIPToSelectedDevice(string ip)
        {
            if (ip == null) return;
            SelectedItem.IPAddresses.Add(ip);
        }

        public void RemoveIPFromSelectedDevice(string ip)
        {
            if (ip == null) return;
            SelectedItem.IPAddresses.Remove(ip);
        }

        private async Task DeleteDeviceAsync()
        {
            try
            {
                await _storage.RemoveDeviceAsync(SelectedItem.InventoryNumber);

                _messagesBroker.NotifyUser($"Устройство \"{SelectedItem.InventoryNumber}\" удалено");

                Device deviceToRemove = SelectedItem;

                RemoveFromList(SelectedItem);

                _appEvents.RaiseDeviceDeleted(deviceToRemove);
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        private async Task LoadDeviceTypesAsync()
        {
            try
            {
                DeviceTypes = await _storage.GetDevicesTypesAsync();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        private async Task AddDeviceAsync()
        {
            try
            {
                var newDevice = new DeviceToCreate {
                    InventoryNumber = InventoryNumber,
                    TypeID = SelectedDeviceType.ID
                };

                await _storage.AddDeviceAsync(newDevice);

                CanAddDevice = false;

                UpdateList();

                SelectedItem = ItemsToDisplay.FirstOrDefault();

                _messagesBroker.NotifyUser(
                    $"Устройство \"{InventoryNumber}\" добавлено"
                );
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
