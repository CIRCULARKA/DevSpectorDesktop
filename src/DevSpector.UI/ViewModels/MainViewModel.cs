using System.Collections.Generic;
using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;
using DevSpector.Desktop.Service;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private int _selectedIndex;

        public MainViewModel(
            DevicesMainView devicesMainView,
            UsersMainView usersMainView,
            SessionBrokerView sessionBrokerView,
            SettingsView settingsView,
            MessagesBrokerView messagesView,
            IUserRights userRights
        ) : base(userRights)
        {
            DevicesMainView = devicesMainView;
            UsersMainView = usersMainView;
            SessionBrokerView = sessionBrokerView;
            SettingsView = settingsView;
            MessagesBrokerView = messagesView;

            Messages = new List<string> {
                "hello",
                "ma",
                "friend"
            };
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedIndex, value);
        }

        public List<string> Messages { get; }

        public UserControl DevicesMainView { get; }

        public UserControl UsersMainView { get; }

        public UserControl SessionBrokerView { get; }

        public UserControl SettingsView { get; }

        public UserControl MessagesBrokerView { get; }

        public void SelectDefaultView()
        {
            SelectedIndex = 0;
        }
    }
}
