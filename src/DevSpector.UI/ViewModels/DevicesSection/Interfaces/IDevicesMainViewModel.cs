using Avalonia.Controls;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IDevicesMainViewModel
    {
        UserControl DevicesList { get; }

        UserControl SoftwareInfo { get; }

        UserControl LocationInfo { get; }

        UserControl NetworkInfo { get; }

        UserControl CommonInfo { get; }

        UserControl Search { get; }
    }
}
