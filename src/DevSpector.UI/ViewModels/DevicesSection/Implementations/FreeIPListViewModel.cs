using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevSpector.Desktop.Service;
using ReactiveUI;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class FreeIPListViewModel : ListViewModelBase<string>, IFreeIPListViewModel
    {
        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IApplicationEvents _appEvents;

        public FreeIPListViewModel(
            IDevicesStorage storage,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListViewModel,
            IApplicationEvents appEvents
        )
        {
            _storage = storage;
            _messagesBroker = messagesBroker;
            _devicesListViewModel = devicesListViewModel;
            _appEvents = appEvents;

            AddFreeIPToDeviceCommand = ReactiveCommand.CreateFromTask(
                AddIPToDeviceAsync,
                this.WhenAny(
                    (vm) => vm.SelectedItem,
                    (vm) => vm._devicesListViewModel.SelectedItem,
                    (freeIp, selectedDevice) => {
                        if (SelectedItem == null) return false;
                        if (_devicesListViewModel.SelectedItem == null) return false;
                        return true;
                    }
                )
            );
        }

        public ReactiveCommand<Unit, Unit> AddFreeIPToDeviceCommand { get; }

        public override string SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public async void UpdateList()
        {
            try
            {
                List<string> updatedList = await _storage.GetFreeIPAsync();

                await Task.Run(() => {
                    ItemsCache.Clear();
                    foreach (var ip in updatedList)
                        ItemsCache.Add(ip);

                    ItemsToDisplay.Clear();
                    foreach (var ip in ItemsCache)
                        ItemsToDisplay.Add(ip);
                });
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        public async Task AddIPToDeviceAsync()
        {
            try
            {
                Device selectedDevice = _devicesListViewModel.SelectedItem;

                await _storage.AddIPAsync(selectedDevice.InventoryNumber, SelectedItem);

                _messagesBroker.NotifyUser(
                    $"IP-адрес \"{SelectedItem}\" был добавлен к устройству \"{selectedDevice.InventoryNumber}\""
                );

                RemoveFromList(SelectedItem);

                _appEvents.RaiseOnIPAddressAdded(device: selectedDevice, ip: SelectedItem);
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

    }
}
