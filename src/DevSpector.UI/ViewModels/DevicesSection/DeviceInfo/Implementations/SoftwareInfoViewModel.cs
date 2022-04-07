using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SoftwareInfoViewModel : ViewModelBase, ISoftwareInfoViewModel
    {
        private string _softwareName;

        private string _softwareVersion;

        private List<Software> _software;

        private Software _selectedSoftware;

        private readonly IDevicesStorage _storage;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IMessagesBroker _messagesBroker;

        public SoftwareInfoViewModel(
            IDevicesStorage storage,
            IDevicesListViewModel devicesListViewModel,
            IMessagesBroker messagesBroker
        )
        {
            _storage = storage;
            _devicesListViewModel = devicesListViewModel;

            _messagesBroker = messagesBroker;

            RemoveSoftwareCommand = ReactiveCommand.CreateFromTask(
                RemoveSoftware
            );
        }

        public ReactiveCommand<Unit, Unit> RemoveSoftwareCommand { get; }

        public string SoftwareName
        {
            get => _softwareName;
            set => this.RaiseAndSetIfChanged(ref _softwareName, value);
        }

        public string SoftwareVersion
        {
            get => _softwareVersion;
            set => this.RaiseAndSetIfChanged(ref _softwareVersion, value);
        }

        public List<Software> Software
        {
            get => _software;
            set => this.RaiseAndSetIfChanged(ref _software, value);
        }

        public Software SelectedSoftware
        {
            get => _selectedSoftware;
            set => this.RaiseAndSetIfChanged(ref _selectedSoftware, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            if (target == null)
                Software = null;
            else
                Software = target.Software;
        }

        public async Task RemoveSoftware()
        {
            try
            {
                Device selectedDevice = _devicesListViewModel.SelectedItem;

                await _storage.RemoveSoftwareAsync(selectedDevice.InventoryNumber, _selectedSoftware);

                Software.Remove(SelectedSoftware);

                Software removedSoftware = SelectedSoftware;

                var temp = Software;
                Software = null;
                Software = temp;

                _messagesBroker.NotifyUser($"ПО \"{removedSoftware.SoftwareName}\" удалено");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
