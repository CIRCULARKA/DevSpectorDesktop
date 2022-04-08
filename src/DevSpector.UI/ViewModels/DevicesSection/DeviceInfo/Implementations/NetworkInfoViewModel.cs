using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class NetworkInfoViewModel : ListViewModelBase<string>, INetworkInfoViewModel
    {
        private bool _canAddIP;

        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IDevicesListViewModel _devicesListViewModel;

        public NetworkInfoViewModel(
            IDevicesStorage storage,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListViewModel
        )
        {
            _storage = storage;
            _messagesBroker = messagesBroker;
            _devicesListViewModel = devicesListViewModel;

            SwitchFreeIPListCommand = ReactiveCommand.Create(
                () => {
                    CanAddIP = !CanAddIP;
                }
            );

            RemoveIPCommand = ReactiveCommand.CreateFromTask(
                RemoveIPFromDeviceAsync,
                this.WhenAny(
                    (vm) => vm.SelectedItem,
                    (vm) => {
                        if (SelectedItem == null) return false;
                        return true;
                    }
                )
            );
        }

        public ReactiveCommand<Unit, Unit> SwitchFreeIPListCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveIPCommand { get; }

        public bool CanAddIP
        {
            get => _canAddIP;
            set => this.RaiseAndSetIfChanged(ref _canAddIP, value);
        }

        public override string SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            if (target == null) return;

            ItemsToDisplay.Clear();
            foreach (var ip in target.IPAddresses)
                ItemsToDisplay.Add(ip);
        }

        public async Task RemoveIPFromDeviceAsync()
        {
            try
            {
                Device selectedDevice = _devicesListViewModel.SelectedItem;

                await _storage.RemoveIPAsync(selectedDevice.InventoryNumber, SelectedItem);

                _messagesBroker.NotifyUser($"IP-адрес \"{SelectedItem}\" удалён у устройства \"{selectedDevice.InventoryNumber}\"");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
