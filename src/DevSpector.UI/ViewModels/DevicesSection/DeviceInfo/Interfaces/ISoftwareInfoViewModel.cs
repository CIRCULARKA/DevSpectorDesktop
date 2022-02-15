using System.Collections.Generic;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ISoftwareInfoViewModel : IDeviceInfoViewModel
    {
        string Software { get; set; }
    }
}
