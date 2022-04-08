using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class NetworkInfoViewModel : ListViewModelBase<string>, INetworkInfoViewModel
    {
        private bool _canAddIP;

        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IApplicationEvents _appEvents;

        public NetworkInfoViewModel(
            IDevicesStorage storage,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListViewModel,
            FreeIPListView freeIPListView,
            IApplicationEvents appEvents
        )
        {
            FreeIPListView = freeIPListView;

            _appEvents = appEvents;

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

        public UserControl FreeIPListView { get; }

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

            ItemsCache.Clear();
            foreach (var ip in target.IPAddresses)
                ItemsCache.Add(ip);

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

                string ipToRemove = SelectedItem;

                RemoveFromList(SelectedItem);

                _appEvents.RaiseOnIPAddressDeleted(_devicesListViewModel.SelectedItem, ipToRemove);
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
