using Avalonia.Controls;
using Ninject;
using DevSpector.Desktop.UI.Views;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class DevicesMainViewModel : ViewModelBase, IDevicesMainViewModel
    {
        public DevicesMainViewModel(
            DevicesListView devicesList,
            CommonInfoView commonInfo,
            NetworkInfoView networkInfo,
            LocationInfoView locationInfo,
            SoftwareInfoView softwareInfo,
            SearchView search,
            IUserRights userRights
        ) : base(userRights)
        {
            DevicesList = devicesList;
            SoftwareInfo = softwareInfo;
            LocationInfo = locationInfo;
            NetworkInfo = networkInfo;
            CommonInfo =  commonInfo;

            Search = search;
        }

        public UserControl DevicesList { get; }

        public UserControl SoftwareInfo { get; }

        public UserControl LocationInfo { get; }

        public UserControl NetworkInfo { get; }

        public UserControl CommonInfo { get; }

        public UserControl Search { get; }
    }
}
