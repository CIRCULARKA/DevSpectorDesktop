using System.Collections.Generic;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface INetworkInfoViewModel : IDeviceInfoViewModel
    {
        List<string> IPAddresses { get; set; }

        string NetworkName { get; set; }
    }
}
