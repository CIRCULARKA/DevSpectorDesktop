using System.Collections.Generic;
using ReactiveUI;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class NetworkInfoViewModel : ViewModelBase, INetworkInfoViewModel
    {
        private string _networkName;

        private List<string> _ipAddresses;

        public NetworkInfoViewModel() { }

        public string NetworkName
        {
            get { return _networkName == null ? "N/A" : _networkName; }
            set => this.RaiseAndSetIfChanged(ref _networkName, value);
        }

        public List<string> IPAddresses
        {
            get => _ipAddresses;
            set => this.RaiseAndSetIfChanged(ref _ipAddresses, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            NetworkName = target?.NetworkName;

            IPAddresses = target?.IPAddresses;
        }
    }
}
