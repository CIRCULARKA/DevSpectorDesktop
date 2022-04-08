using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IDevicesListViewModel : IListViewModel<Device>, IInitializableListViewModel<Device>
    {
        void AddIPToSelectedDevice(string ip);

        void RemoveIPFromSelectedDevice(string ip);
    }
}
