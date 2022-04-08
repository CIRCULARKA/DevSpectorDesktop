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
        private ObservableCollection<string> _freeIP;

        private string _selectedFreeIP;

        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IDevicesListViewModel _devicesListViewModel;

        public NetworkInfoViewModel(
            IDevicesStorage storage,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListViewModel
        )
        {
            _freeIP = new ObservableCollection<string>();

            AddFreeIPToDeviceCommand = ReactiveCommand.CreateFromTask(
                AddIPToDeviceAsync,
                this.WhenAny(
                    (vm) => vm.SelectedFreeIP,
                    (vm) => vm._devicesListViewModel.SelectedItem,
                    (freeIp, selectedDevice) => {
                        if (SelectedFreeIP == null) return false;
                        if (_devicesListViewModel.SelectedItem == null) return false;
                        return true;
                    }
                )
            );

            _storage = storage;
            _messagesBroker = messagesBroker;
            _devicesListViewModel = devicesListViewModel;
        }

        public ReactiveCommand<Unit, Unit> AddFreeIPToDeviceCommand { get; }

        public ObservableCollection<string> FreeIP => _freeIP;

        public string SelectedFreeIP
        {
            get => _selectedFreeIP;
            set => this.RaiseAndSetIfChanged(ref _selectedFreeIP, value);
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

        public async Task AddIPToDeviceAsync()
        {
            try
            {
                Device selectedDevice = _devicesListViewModel.SelectedItem;

                await _storage.AddIPAsync(selectedDevice.InventoryNumber, SelectedFreeIP);

                _messagesBroker.NotifyUser(
                    $"IP-адрес \"{SelectedFreeIP}\" был добавлен к устройству \"{selectedDevice.InventoryNumber}\""
                );
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        public async Task LoadFreeIP()
        {
            try
            {
                List<string> updatedList = await _storage.GetFreeIPAsync();

                await Task.Run(() => {
                    FreeIP.Clear();
                    foreach (var ip in updatedList)
                        FreeIP.Add(ip);
                });
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
