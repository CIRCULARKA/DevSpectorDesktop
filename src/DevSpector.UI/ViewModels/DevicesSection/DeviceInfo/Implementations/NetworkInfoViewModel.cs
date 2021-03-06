using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.SDK.Networking;
using DevSpector.Desktop.Service;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class NetworkInfoViewModel : ListViewModelBase<string>, INetworkInfoViewModel
    {
        private bool _canAddIP;

        private readonly INetworkManager _networkManager;

        private readonly IDevicesEditor _devicesEditor;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly ApplicationEvents _appEvents;

        public NetworkInfoViewModel(
            INetworkManager networkManager,
            IDevicesEditor devicesEditor,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListViewModel,
            FreeIPListView freeIPListView,
            ApplicationEvents appEvents,
            IUserRights userRights
        ) : base(userRights)
        {
            FreeIPListView = freeIPListView;

            _appEvents = appEvents;

            _networkManager = networkManager;
            _devicesEditor = devicesEditor;
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
            if (target == null)
            {
                ItemsCache.Clear();
                ItemsToDisplay.Clear();
                return;
            }

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

                await _devicesEditor.RemoveIPAsync(selectedDevice.InventoryNumber, SelectedItem);

                _messagesBroker.NotifyUser($"IP-?????????? \"{SelectedItem}\" ???????????? ?? ???????????????????? \"{selectedDevice.InventoryNumber}\"");

                string ipToRemove = SelectedItem;

                RemoveFromList(SelectedItem);

                _appEvents.RaiseOnIPAddressDeleted(_devicesListViewModel.SelectedItem, ipToRemove);
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        public void HideFreeIPList()
        {
            CanAddIP = false;
        }
    }
}
