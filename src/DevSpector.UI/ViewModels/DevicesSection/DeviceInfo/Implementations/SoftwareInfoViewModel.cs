using System.Collections.Generic;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SoftwareInfoViewModel : ViewModelBase, ISoftwareInfoViewModel
    {
        private List<Software> _software;

        private readonly IDevicesStorage _storage;

        private readonly IDevicesListViewModel _devicesListViewModel;

        public SoftwareInfoViewModel(
            IDevicesStorage storage,
            IDevicesListViewModel devicesListViewModel
        )
        {
            _storage = storage;
            _devicesListViewModel = devicesListViewModel;
        }

        public List<Software> Software
        {
            get => _software;
            set => this.RaiseAndSetIfChanged(ref _software, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            if (target == null)
                Software = null;
            else
                Software = target.Software;
        }
    }
}
