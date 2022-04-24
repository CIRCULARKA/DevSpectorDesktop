using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.SDK.Providers;
using DevSpector.Desktop.Service;
using DevSpector.Desktop.UI.Validators;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class DevicesListViewModel : ListViewModelBase<Device>, IDevicesListViewModel
    {
        private readonly ApplicationEvents _appEvents;

        private readonly IUserSession _session;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IDevicesEditor _devicesEditor;

        private readonly IDevicesProvider _devicesProvider;

        private string _inventoryNumber;

        private string _modelName;

        private bool _canAddDevice;

        private DeviceType _selectedDeviceType;

        private List<DeviceType> _deviceTypes;

        private readonly ITextValidator _textValidator;

        public DevicesListViewModel(
            IDevicesEditor devicesEditor,
            IDevicesProvider devicesProvider,
            ApplicationEvents appEvents,
            IUserSession session,
            IMessagesBroker messagesBroker,
            IUserRights userRights,
            ITextValidator textValidator
        ) : base(userRights)
        {
            _appEvents = appEvents;
            _session = session;
            _textValidator = textValidator;

            _messagesBroker = messagesBroker;

            _devicesEditor = devicesEditor;
            _devicesProvider = devicesProvider;

            SwitchInputFieldsCommand = ReactiveCommand.CreateFromTask(
                async () => {
                    CanAddDevice = !CanAddDevice;

                    if (!CanAddDevice) return;

                    await LoadDeviceTypesAsync();
                    SelectedDeviceType = DeviceTypes.FirstOrDefault();
                }
            );

            AddDeviceCommand = ReactiveCommand.CreateFromTask(
                AddDeviceAsync,
                this.WhenAny(
                    (vm) => vm.InventoryNumber,
                    (vm) => vm.SelectedDeviceType,
                    (invNum, deviceType) => {
                        if (SelectedDeviceType == null) return false;
                        return _textValidator.IsValid(InventoryNumber);
                    }
                )
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
                _textValidator.Validate(value);
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

            ItemsCache = await _devicesProvider.GetDevicesAsync();
            ItemsToDisplay.Clear();
            foreach (var device in ItemsCache)
                ItemsToDisplay.Add(device);
        }

        public async Task UpdateListAsync(object keyToSelectBy = null)
        {
            try
            {
                await LoadItems();

                if (ItemsToDisplay.Count > 0)
                {
                    AreThereItems = true;
                    if (keyToSelectBy == null)
                        SelectedItem = ItemsToDisplay[0];
                    else
                        SelectedItem = ItemsToDisplay.FirstOrDefault(d => d.ID == (Guid)keyToSelectBy);
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
                await _devicesEditor.DeleteDeviceAsync(SelectedItem.InventoryNumber);

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
                DeviceTypes = await _devicesProvider.GetDeviceTypesAsync();
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

                await _devicesEditor.CreateDeviceAsync(newDevice);

                _messagesBroker.NotifyUser(
                    $"Устройство \"{InventoryNumber}\" добавлено"
                );

                await UpdateListAsync();

                SelectedItem = ItemsToDisplay.FirstOrDefault(d => d.InventoryNumber == InventoryNumber);

                ClearInput();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        private void ClearInput()
        {
            CanAddDevice = false;
            SelectedDeviceType = DeviceTypes.FirstOrDefault();
            InventoryNumber = string.Empty;
        }
    }
}
