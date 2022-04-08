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

            _storage = storage;
            _messagesBroker = messagesBroker;
            _devicesListViewModel = devicesListViewModel;

            SwitchFreeIPListCommand = ReactiveCommand.Create(
                () => {
                    CanAddIP = !CanAddIP;
                }
            );
        }

        public ReactiveCommand<Unit, Unit> SwitchFreeIPListCommand { get; }

        public ObservableCollection<string> FreeIP => _freeIP;

        public bool CanAddIP
        {
            get => _canAddIP;
            set => this.RaiseAndSetIfChanged(ref _canAddIP, value);
        }

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
