using System;
using System.Linq;
using Avalonia.Controls;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class CommonInfoViewModel : ViewModelBase, ICommonInfoViewModel
    {
        private string _inventoryNumber;

        private string _type;

        private List<DeviceType> _deviceTypes;

        private DeviceType _selectedDeviceType;

        private readonly IDevicesStorage _storage;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IMessagesBroker _messagesBroker;

        public CommonInfoViewModel(
            IDevicesStorage storage,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListVM
        )
        {
            _storage = storage;
            _devicesListViewModel = devicesListVM;

            _messagesBroker = messagesBroker;

            ApplyChangesCommand = ReactiveCommand.CreateFromTask(
                async () => {
                    await _storage.UpdateDeviceAsync(
                        _devicesListViewModel.SelectedItem.InventoryNumber,
                        new DeviceToCreate {
                            InventoryNumber = InventoryNumber,
                            TypeID = SelectedDeviceType.ID
                        }
                    );
                },
                this.WhenAny(
                    (vm) => vm.InventoryNumber,
                    (vm) => vm.SelectedDeviceType,
                    (invNum, deviceType) => {
                        Device selectedDevice = _devicesListViewModel.SelectedItem;

                        if (selectedDevice == null) return false;
                        if (SelectedDeviceType == null) return false;

                        return InventoryNumber != selectedDevice.InventoryNumber ||
                            SelectedDeviceType.Name != selectedDevice.Type;
                    }
                )
            );
        }

        public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

        public string InventoryNumber
        {
            get { return _inventoryNumber == null ? "N/A" : _inventoryNumber; }
            set => this.RaiseAndSetIfChanged(ref _inventoryNumber, value);
        }

        public string Type
        {
            get { return _type == null ? "N/A" : _type; }
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        public List<DeviceType> DeviceTypes
        {
            get => _deviceTypes;
            set => this.RaiseAndSetIfChanged(ref _deviceTypes, value);
        }

        public DeviceType SelectedDeviceType
        {
            get => _selectedDeviceType;
            set => this.RaiseAndSetIfChanged(ref _selectedDeviceType, value);
        }

        public async Task UpdateDeviceTypesAsync()
        {
            try
            {
                DeviceTypes = await _storage.GetDevicesTypesAsync();
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
            Type = target?.Type;
        }
    }
}
