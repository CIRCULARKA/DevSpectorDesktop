using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Editors;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class CommonInfoViewModel : ViewModelBase, ICommonInfoViewModel
    {
        private string _inventoryNumber;

        private string _modelName;

        private string _networkName;

        private bool _canEditDevice;

        private List<DeviceType> _deviceTypes;

        private DeviceType _selectedDeviceType;

        private readonly IDevicesProvider _devicesProvider;

        private readonly IDevicesEditor _devicesEditor;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IMessagesBroker _messagesBroker;

        private readonly ApplicationEvents _appEvents;

        public CommonInfoViewModel(
            IDevicesProvider devicesProvider,
            IDevicesEditor devicesEditor,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListVM,
            ApplicationEvents appEvents,
            IUserRights userRights
        ) : base(userRights)
        {
            _devicesProvider = devicesProvider;
            _devicesEditor = devicesEditor;
            _devicesListViewModel = devicesListVM;

            _messagesBroker = messagesBroker;

            _appEvents = appEvents;

            ApplyChangesCommand = ReactiveCommand.CreateFromTask(
                UpdateDeviceCommonInfo,
                this.WhenAny(
                    (vm) => vm.InventoryNumber,
                    (vm) => vm.SelectedDeviceType,
                    (vm) => vm.ModelName,
                    (vm) => vm.NetworkName,
                    (invNum, deviceType, modelName, networkName) => {
                        Device selectedDevice = _devicesListViewModel.SelectedItem;

                        if (selectedDevice == null) return false;
                        if (SelectedDeviceType == null) return false;

                        return InventoryNumber != selectedDevice.InventoryNumber ||
                            SelectedDeviceType.Name != selectedDevice.Type ||
                            ModelName != selectedDevice.ModelName ||
                            NetworkName != selectedDevice.NetworkName;
                    }
                )
            );
        }

        public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

        public string InventoryNumber
        {
            get => _inventoryNumber;
            set => this.RaiseAndSetIfChanged(ref _inventoryNumber, value);
        }

        public string ModelName
        {
            get => _modelName;
            set => this.RaiseAndSetIfChanged(ref _modelName, value);
        }

        public string NetworkName
        {
            get => _networkName;
            set => this.RaiseAndSetIfChanged(ref _networkName, value);
        }

        public List<DeviceType> DeviceTypes
        {
            get => _deviceTypes;
            set => this.RaiseAndSetIfChanged(ref _deviceTypes, value);
        }

        public bool CanEditDevice
        {
            get => _canEditDevice;
            set => this.RaiseAndSetIfChanged(ref _canEditDevice, value);
        }

        public DeviceType SelectedDeviceType
        {
            get => _selectedDeviceType;
            set => this.RaiseAndSetIfChanged(ref _selectedDeviceType, value);
        }

        public async Task LoadDeviceTypesAsync()
        {
            try
            {
                DeviceTypes = await _devicesProvider.GetDeviceTypesAsync();
                SelectedDeviceType = _deviceTypes.FirstOrDefault();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        public void UpdateDeviceInfo(Device target)
        {
            InventoryNumber = target?.InventoryNumber;
            if (DeviceTypes != null)
                SelectedDeviceType = DeviceTypes.FirstOrDefault(dt => dt.Name == target?.Type);
            ModelName = target?.ModelName;
            NetworkName = target?.NetworkName;
        }

        public void UpdateInputsAccessibility() =>
            CanEditDevice =  _devicesListViewModel.SelectedItem != null;

        private async Task UpdateDeviceCommonInfo()
        {
            Device selectedDevice = _devicesListViewModel.SelectedItem;

            try
            {
                string newInventoryNumber = selectedDevice.InventoryNumber == InventoryNumber ?
                    null : InventoryNumber;

                string newType = selectedDevice.Type == SelectedDeviceType.Name ?
                    null : SelectedDeviceType.ID;

                string newModelName = selectedDevice.ModelName == ModelName ?
                    null : ModelName;

                string newNetworkName = selectedDevice.NetworkName == NetworkName ?
                    null : NetworkName;

                await _devicesEditor.UpdateDeviceAsync(
                    selectedDevice.InventoryNumber,
                    new DeviceToCreate {
                        InventoryNumber = newInventoryNumber,
                        ModelName = newModelName,
                        NetworkName = newNetworkName,
                        TypeID = newType
                    }
                );

                _appEvents.RaiseDeviceUpdated(selectedDevice.ID);

                _messagesBroker.NotifyUser($"Устройство \"{selectedDevice.InventoryNumber}\" успешно обновлено");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);

                InventoryNumber = selectedDevice.InventoryNumber;
                ModelName = selectedDevice.ModelName;
                NetworkName = selectedDevice.NetworkName;
                SelectedDeviceType = DeviceTypes.FirstOrDefault(dt => dt.Name == selectedDevice.Type);
            }
        }
    }
}
