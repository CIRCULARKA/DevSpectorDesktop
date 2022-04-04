using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(
            DevicesMainView devicesMainView,
            UsersMainView usersMainView,
            SessionBrokerView sessionBrokerView,
            SettingsView settingsView
        )
        {
            DevicesMainView = devicesMainView;
            UsersMainView = usersMainView;
            SessionBrokerView = sessionBrokerView;
            SettingsView = settingsView;
        }

        public UserControl DevicesMainView { get; }

        public UserControl UsersMainView { get; }

        public UserControl SessionBrokerView { get; }

        public UserControl SettingsView { get; }
    }
}
