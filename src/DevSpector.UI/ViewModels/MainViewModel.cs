using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel(
            DevicesMainView devicesMainView,
            UsersMainView usersMainView,
            SessionBrokerView sessionBrokerView
        )
        {
            DevicesMainView = devicesMainView;
            UsersMainView = usersMainView;
            SessionBrokerView = sessionBrokerView;
        }

        public UserControl DevicesMainView { get; }

        public UserControl UsersMainView { get; }

        public UserControl SessionBrokerView { get; }
    }
}
